using Caliburn.Micro;
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
    }
}
