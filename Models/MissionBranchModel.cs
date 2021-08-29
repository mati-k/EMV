using Caliburn.Micro;
using EMV.Exceptions;
using EMV.Models.Files;
using EMV.SharedData;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Models
{
    public class MissionBranchModel : PropertyChangedBase, IParadoxRead
    {
        private string _name;
        private int _slot = 1;
        private bool _generic = false;
        private bool _ai = true;
        private bool _countryShield = true;
        private NodeModel _potential;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }
        public int Slot
        {
            get { return _slot; }
            set
            {
                _slot = value;
                NotifyOfPropertyChange(() => Slot);
            }
        }
        public bool Generic
        {
            get { return _generic; }
            set
            {
                _generic = value;
                NotifyOfPropertyChange(() => Generic);
            }
        }
        public bool AI
        {
            get { return _ai; }
            set
            {
                _ai = value;
                NotifyOfPropertyChange(() => AI);
            }
        }
        public bool CountryShield
        {
            get { return _countryShield; }
            set
            {
                _countryShield = value;
                NotifyOfPropertyChange(() => CountryShield);
            }
        }
        public NodeModel Potential
        {
            get { return _potential; }
            set 
            { 
                _potential = value;
                NotifyOfPropertyChange(() => Potential);
            }
        }
        public BindableCollection<MissionModel> Missions { get; set; } = new BindableCollection<MissionModel>();
        public MissionFileModel MissionFile { get; set; }

        public MissionBranchModel(MissionFileModel missionFile)
        {
            this.MissionFile = missionFile;
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "slot": Slot = parser.ReadInt32(); break;
                case "generic": Generic = parser.ReadBool(); break;
                case "ai": AI = parser.ReadBool(); break;
                case "potential": Potential = parser.Parse(new GroupNodeModel() { Name = "potential" }); break;
                case "has_country_shield": CountryShield = parser.ReadBool(); break;
                default: Missions.Add(parser.Parse(new MissionModel(this) { Name = token })); break;
            }
        }

        private string GetValueNodeText(ValueNodeModel node)
        {
            return node.Name + " " + ((ValueNodeModel)node).Value;
        }

        public List<string> GetPotentialTagsAndFlags()
        {
            return GetPotentialTagsAndFlags(Potential as GroupNodeModel);
        }

        private List<string> GetPotentialTagsAndFlags(GroupNodeModel localRoot)
        {
            List<string> flags = new List<string>();

            foreach (NodeModel node in localRoot.Nodes)
            {
                if (node is ValueNodeModel)
                    flags.Add(GetValueNodeText(node as ValueNodeModel));
                else
                    flags.AddRange(GetPotentialTagsAndFlags(node as GroupNodeModel));
            }

            return flags;
        }

        public bool IsBranchValid(BindableCollection<Flag> flags)
        {
            return ValidateAnd(flags, Potential as GroupNodeModel);
        }

        private bool ValidateGroup(BindableCollection<Flag> flags, GroupNodeModel root)
        {
            if (root.Name.ToUpper().Equals("AND"))
                return ValidateAnd(flags, root);
            else if (root.Name.ToUpper().Equals("OR"))
                return ValidateOr(flags, root);
            else if (root.Name.ToUpper().Equals("NOT"))
                return !ValidateOr(flags, root);

            return true;
        }

        private bool ValidateAnd(BindableCollection<Flag> flags, GroupNodeModel root)
        {
            foreach (NodeModel node in root.Nodes)
            {
                if (node is ValueNodeModel)
                {
                    string text = GetValueNodeText(node as ValueNodeModel);
                    if (!flags.First(f => f.Value.Equals(text)).Enabled)
                        return false;
                }

                else if (!ValidateGroup(flags, node as GroupNodeModel))
                    return false;
            }

            return true;
        }

        private bool ValidateOr(BindableCollection<Flag> flags, GroupNodeModel root)
        {
            foreach (NodeModel node in (Potential as GroupNodeModel).Nodes)
            {
                if (node is ValueNodeModel)
                {
                    string text = GetValueNodeText(node as ValueNodeModel);
                    if (flags.First(f => f.Value.Equals(text)).Enabled)
                        return true;
                }

                else if (ValidateGroup(flags, node as GroupNodeModel))
                    return true;
            }

            return true;
        }
    }
}
