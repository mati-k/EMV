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
        public BindableCollection<Flag> Flags { get; set; } = new BindableCollection<Flag>();

        public override void TokenCallback(ParadoxParser parser, string token)
        {
            Branches.Add(parser.Parse(new MissionBranchModel(this) { Name = token }));
        }

        public override void BindLocalisation(Dictionary<string, string> localisation)
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

        public void FindPotentialTagsAndFlags()
        {
            foreach (MissionBranchModel branch in Branches)
            {
                List<string> flags = branch.GetPotentialTagsAndFlags();
                foreach (string flag in flags)
                {
                    if (!Flags.Any(f => f.Value.Equals(flag)))
                        Flags.Add(new Flag(flag));
                }
            }

            if (Flags.Count > 0)
                Flags[0].Enabled = true;
        }
    }
}