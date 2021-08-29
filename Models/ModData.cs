using Caliburn.Micro;
using EMV.Models.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Models
{
    public class ModData : IModData
    {
        public BindableCollection<MissionFileModel> MissionFiles { get; set; }
       
        public ModData()
        {
            this.MissionFiles = new BindableCollection<MissionFileModel>();
        }

        public void BindLocalisation(Dictionary<string, string> localisation)
        {
            foreach (MissionFileModel missionFile in MissionFiles)
            {
                missionFile.BindLocalisation(localisation);
                missionFile.FindPotentialTagsAndFlags();
            }

            List<string> used = new List<string>();
        }
    }
}
