using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Newtonsoft.Json;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace EMV.Models
{
    [DataContract]
    public class FilesModel : PropertyChangedBase
    {
        private const string pathsFile = "paths.json";

        private string _vanillaFolder;
        private string _modFolder;

        [DataMember]
        public string VanillaFolder
        {
            get { return _vanillaFolder; }
            set
            {
                _vanillaFolder = value;
                NotifyOfPropertyChange(() => VanillaFolder);
            }
        }

        [DataMember]
        public string ModFolder
        {
            get { return _modFolder; }
            set
            {
                _modFolder = value;
                NotifyOfPropertyChange(() => ModFolder);
            }
        }

        public FilesModel()
        {

        }

        public FilesModel(string vanillaFolder, string modFolder)
        {
            this.VanillaFolder = vanillaFolder;
            this.ModFolder = modFolder;
        }

        public void SaveToJson()
        {
            string file = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(pathsFile, file);
        }

        public static FilesModel ReadFromJson()
        {
            try
            {
                string file = File.ReadAllText(pathsFile);
                FilesModel filesModel = JsonConvert.DeserializeObject<FilesModel>(file);
                
                return filesModel;
            }

            catch (Exception e)
            {
                return new FilesModel();
            }
        }
    }
}
