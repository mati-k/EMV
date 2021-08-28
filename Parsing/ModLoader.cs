using Caliburn.Micro;
using EMV.Models;
using EMV.Models.Files;
using EMV.SharedData;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Parsing
{
    public class ModLoader : IModLoader
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private FilesModel filesModel;

        public void LoadModData(IModData mod, FilesModel files)
        {
            this.filesModel = files;

            mod.MissionFiles.AddRange(LoadModFolder<MissionFileModel>("missions").Select(pair => pair.Value));
            
            LoadGfx();
            Dictionary<string, string> localisation = LoadLocalisation();

            mod.BindLocalisation(localisation);
        }

        private Dictionary<string, T> LoadModFolder<T>(string folder, bool includeVanilla = false) where T : ModFileBase, new()
        {
            Dictionary<string, T> data = new Dictionary<string, T>();
           
            List<string> folders = new List<string>();
            folders.Add(Path.Combine(filesModel.ModFolder, folder));
            if (includeVanilla)
                folders.Add(Path.Combine(filesModel.VanillaFolder, folder));

            List<string> files = GetAllFolderAndSubfolders(folders)
                                .SelectMany(folder => Directory.EnumerateFiles(folder))
                                .Where(f => Path.GetExtension(f).Equals(".txt")).ToList();

            foreach (string file in files)
            {
                try
                {
                    using (FileStream fileStream = new FileStream(file, FileMode.Open))
                    {
                        T fileData = ParadoxParser.Parse(fileStream, new T());
                        fileData.FileName = Path.GetFileNameWithoutExtension(file);


                        if (!data.ContainsKey(fileData.FileName))
                            data.Add(fileData.FileName, fileData);
                    }
                }
                catch (Exception e)
                {
                    log.Error(String.Format("Error loading {0}", file), e);
                }
            }

            return data;
        }
        
        private void LoadGfx()
        {
            Dictionary<string, string> gfxFiles = new Dictionary<string, string>();
            List<string> files = GetAllFolderAndSubfolders(new List<string>() { Path.Combine(filesModel.ModFolder, "interface"), Path.Combine(filesModel.VanillaFolder, "interface") })
                                .SelectMany(folder => Directory.EnumerateFiles(folder)).Where(f => Path.GetExtension(f).Equals(".gfx")).ToList();

            foreach (string gfxFile in files)
            {
                try
                {
                    using (FileStream fileStream = new FileStream(gfxFile, FileMode.Open))
                    {
                        GfxFileModel gfxFileData = ParadoxParser.Parse(fileStream, new GfxFileModel());
                        string rootDirectory = gfxFile;
                        while (!(rootDirectory.Equals(filesModel.ModFolder) || rootDirectory.Equals(filesModel.VanillaFolder)))
                            rootDirectory = Directory.GetParent(rootDirectory).FullName;

                        if (gfxFile.Contains("core.gfx") && !FontColors.Instance.Colors.Any()) // skip if mod added
                        {
                            var colors = gfxFileData.OtherGfx.Where(b => b.Name.Equals("bitmapfonts")).First()
                                .Nodes.Where(n => n.Name.Equals("textcolors")).First().Nodes;

                            foreach (var color in colors)
                            {
                                FontColors.Instance.Colors.Add(new ColorKey(color.Name[0], color.Colors));
                            }
                        }

                        gfxFileData.Gfx.ToList().ForEach(gfx =>
                        {
                            if (gfx.TextureFile != null && !gfxFiles.ContainsKey(gfx.Name))
                            {
                                if (gfx.TextureFile.Replace(@"//", @"/").StartsWith("gfx/interface/missions"))
                                    gfxFiles.Add(gfx.Name, Path.Combine(rootDirectory, gfx.TextureFile));
                                if (gfx.Name.Equals("GFX_mission_icons_frame"))
                                    StaticPaths.Instance.MissionFramePath = Path.Combine(rootDirectory, gfx.TextureFile);
                            }
                        });
                    }
                }
                catch (Exception e)
                {
                    log.Error(String.Format("Loading gfx {0}", gfxFile), e);
                }
            }

            GfxStorage.Instance.GfxFiles = gfxFiles;
        }

        public Dictionary<string, string> LoadLocalisation()
        {
            Dictionary<string, string> localisation = new Dictionary<string, string>();

            List<string> files = GetAllFolderAndSubfolders(new List<string>() { Path.Combine(filesModel.ModFolder, "localisation"), Path.Combine(filesModel.VanillaFolder, "localisation") })
                    .SelectMany(folder => Directory.EnumerateFiles(folder)).Where(f => Path.GetExtension(f).Equals(".yml")).ToList();

            foreach (string file in files)
            {
                try
                {
                    using (FileStream fileStream = new FileStream(file, FileMode.Open))
                    {
                        Localisation.Read(new StreamReader(fileStream)).ForEach(tuple =>
                        {
                            if (!localisation.ContainsKey(tuple.Item1))
                                localisation.Add(tuple.Item1, tuple.Item2);
                            else
                                Console.WriteLine("Duplicate localisation: {0} = {1}", tuple.Item1, tuple.Item2); // Save to file?
                    });
                    }
                }

                catch (Exception e)
                {
                    log.Error(string.Format("Error loading localisation file {0}", file), e);
                }
            }

            return localisation;
        }

        private List<string> GetAllFolderAndSubfolders(List<string> rootFolders)
        {
            List<string> folders = new List<string>();

            foreach (string folder in rootFolders)
            {
                folders.Add(folder);

                List<string> subDirectories = Directory.GetDirectories(folder).ToList();
                if (subDirectories.Count > 0)
                {
                    folders.AddRange(GetAllFolderAndSubfolders(subDirectories));
                }
            }

            return folders;
        }

    }
}
