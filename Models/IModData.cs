using Caliburn.Micro;
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
    }
}
