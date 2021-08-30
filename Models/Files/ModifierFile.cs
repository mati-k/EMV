using Caliburn.Micro;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Models.Files
{
    public class ModifierFile : ModFileBase
    {
        public BindableCollection<Modifier> Modifiers { get; set; } = new BindableCollection<Modifier>();

        public override void TokenCallback(ParadoxParser parser, string token)
        {
            Modifiers.Add(parser.Parse(new Modifier() { Name = token }));
        }

        public override void BindLocalisation(Dictionary<string, string> localisation)
        {
            foreach (Modifier modifier in Modifiers)
            {
                if (localisation.ContainsKey(modifier.Name))
                {
                    modifier.Title = localisation[modifier.Name];
                }

                if (localisation.ContainsKey("desc_" + modifier.Name))
                {
                    modifier.Description = localisation["desc_" + modifier.Name];
                }
            }
        }
    }
}
