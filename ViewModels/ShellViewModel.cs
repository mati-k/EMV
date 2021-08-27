using Caliburn.Micro;
using EMV.Converters;
using EMV.Models;
using EMV.Views;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using EMV.SharedData;
using System.Windows.Media;
using EMV.Handlers;

namespace EMV.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<FilesModel>, IHandle<WindowMessage>
    {
        private IEventAggregator _eventAggregator;
        private IWindowManager _windowManager;

        private static readonly log4net.ILog log = LogHelper.GetLogger();

        private StartViewModel _startViewModel;
        private MenuViewModel _menuViewModel;
        private IModData _mod;

        private Dictionary<string, string> _unconnectedLocalisation = new Dictionary<string, string>();

        private FilesModel _filesModel;

        public IModData Mod
        {
            get { return _mod; }
            set 
            {
                _mod = value;
                NotifyOfPropertyChange(() => Mod);
            }
        }

        public ShellViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, StartViewModel startViewModel, MenuViewModel menuViewModel, IModData mod)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);

            _mod = mod;

            _startViewModel = startViewModel;
            _menuViewModel = menuViewModel;

            ActivateItemAsync(startViewModel, CancellationToken.None);
        }

        public void Save()
        {

        }

        public Task HandleAsync(FilesModel message, CancellationToken cancellationToken)
        {
            _filesModel = message;
            MissionFileModel missionFileModel;

            if (!Directory.Exists(Path.Combine(message.ModFolder, "interface")))
            {
                MessageDialogViewModel dialog = IoC.Get<MessageDialogViewModel>();
                dialog.Message = "Mod folder doesn't have interface folder";
                _windowManager.ShowDialogAsync(dialog);
                return Task.CompletedTask;
            }

            if (!Directory.Exists(Path.Combine(message.VanillaFolder, "interface")))
            {
                MessageDialogViewModel dialog = IoC.Get<MessageDialogViewModel>();
                dialog.Message = "Vanilla folder doesn't have interface folder";
                _windowManager.ShowDialogAsync(dialog);
                return Task.CompletedTask;
            }
            LoadMissions(message.VanillaFolder, message.ModFolder);

            LoadGfx(message.VanillaFolder, message.ModFolder);

            return ActivateItemAsync(_menuViewModel, CancellationToken.None);
        }

        private void LoadMissions(string vanillaFolder, string modFolder)
        {
            Dictionary<string, string> missionFiles = new Dictionary<string, string>();
            List<string> files = GetAllFolderAndSubfolders(new List<string>() { Path.Combine(modFolder, "missions"), Path.Combine(vanillaFolder, "interface") })
                                .SelectMany(folder => Directory.EnumerateFiles(folder)).Where(f => Path.GetExtension(f).Equals(".txt")).ToList();

            foreach (string file in files)
            {
                try
                {
                    using (FileStream fileStream = new FileStream(file, FileMode.Open))
                    {
                        MissionFileModel missionFile = ParadoxParser.Parse(fileStream, new MissionFileModel() { FileName = Path.GetFileNameWithoutExtension(file) });
                        Mod.MissionFiles.Add(missionFile);
                    }
                }
                catch (Exception e)
                {
                    log.Error(String.Format("Loading mission {0}", file), e);
                }
            }
        }

        private void LoadGfx(string vanillaFolder, string modFolder)
        {
            Dictionary<string, string> gfxFiles = new Dictionary<string, string>();
            List<string> files = GetAllFolderAndSubfolders(new List<string>() { Path.Combine(modFolder, "interface"), Path.Combine(vanillaFolder, "interface") })
                                .SelectMany(folder => Directory.EnumerateFiles(folder)).Where(f => Path.GetExtension(f).Equals(".gfx")).ToList();

            foreach (string gfxFile in files)
            {
                try
                {
                    using (FileStream fileStream = new FileStream(gfxFile, FileMode.Open))
                    {
                        GfxFileModel gfxFileData = ParadoxParser.Parse(fileStream, new GfxFileModel());
                        string rootDirectory = gfxFile;
                        while (!(rootDirectory.Equals(modFolder) || rootDirectory.Equals(vanillaFolder)))
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

        public Task HandleAsync(WindowMessage message, CancellationToken cancellationToken)
        {
            return ActivateItemAsync(IoC.GetInstance(message.ViewModelType, null));
        }
    }
}
