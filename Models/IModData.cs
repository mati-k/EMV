using Caliburn.Micro;
using EMV.Models.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Models
{
    public interface IModData
    {
        public BindableCollection<MissionFileModel> MissionFiles { get; set; }
        public BindableCollection<ModifierFile> ModifierFiles { get; set; }
        public BindableCollection<EventFile> EventFiles { get; set; }

        public void BindLocalisation(Dictionary<string, string> localisation);
    }
}
