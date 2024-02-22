using Caliburn.Micro;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Models.Files
{
    public class EventFile : ModFileBase
    {
        public BindableCollection<ModEvent> Events { get; set; } = new BindableCollection<ModEvent>();
        public string Namespace { get; set; }

        public override void TokenCallback(ParadoxParser parser, string token)
        {
            if (token.Equals("namespace"))
                Namespace = parser.ReadString();
            else if (token.Equals("country_event"))
                Events.Add(parser.Parse(new ModEvent(true)));
            else
                Events.Add(parser.Parse(new ModEvent(false)));
        }

        public override void BindLocalisation(Dictionary<string, string> localisation)
        {
            foreach (ModEvent modEvent in Events)
            {
                if (modEvent.Title != null && localisation.ContainsKey(modEvent.Title))
                {
                    modEvent.Title = localisation[modEvent.Title];
                }

                if (modEvent.Description != null && localisation.ContainsKey(modEvent.Description))
                {
                    modEvent.Description = localisation[modEvent.Description];
                }

                foreach (EventOption option in modEvent.Options)
                {
                    if (localisation.ContainsKey(option.Name))
                        option.Name = localisation[option.Name];
                }
            }
        }
    }
}
