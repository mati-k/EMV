using Caliburn.Micro;
using EMV.Exceptions;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Models.Files
{
    public class MissionFileModel : ModFileBase
    {
        public BindableCollection<MissionBranchModel> Branches { get; set; } = new BindableCollection<MissionBranchModel>();

        public override void TokenCallback(ParadoxParser parser, string token)
        {
            Branches.Add(parser.Parse(new MissionBranchModel(this) { Name = token }));
        }

        public void BindLocalisation(Dictionary<string, string> localisation)
        {
            foreach (MissionBranchModel branch in Branches)
            {
                foreach (MissionModel mission in branch.Missions)
                {
                    if (localisation.ContainsKey(mission.Name + "_title"))
                    {
                        mission.Title = localisation[mission.Name + "_title"];
                    }

                    if (localisation.ContainsKey(mission.Name + "_desc"))
                    {
                        mission.Description = localisation[mission.Name + "_desc"];
                    }
                }
            }
        }
    }
}