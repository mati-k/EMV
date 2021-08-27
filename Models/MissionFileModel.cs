using Caliburn.Micro;
using EMV.Exceptions;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Models
{
    public class MissionFileModel : PropertyChangedBase, IParadoxRead
    {
        public string FileName { get; set; }
        public BindableCollection<MissionBranchModel> Branches { get; set; } = new BindableCollection<MissionBranchModel>();

        public void TokenCallback(ParadoxParser parser, string token)
        {
            Branches.Add(parser.Parse(new MissionBranchModel(this) { Name = token }));
        }
    }
}