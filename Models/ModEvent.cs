using Caliburn.Micro;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Models
{
    public class ModEvent : IParadoxRead
    {
        public string Id { get; set; }
        public bool IsCountryEvent { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public bool FireOnlyOnce { get; set; } = false;
        public bool IsTriggeredOnly { get; set; } = false;
        public bool Hidden { get; set; } = false;
        public string GoTo { get; set; }

        public GroupNodeModel MeanTimeToHappen { get; set; }
        public GroupNodeModel Trigger { get; set; }
        public GroupNodeModel Immediate { get; set; }
        public BindableCollection<EventOption> Options { get; set; } = new BindableCollection<EventOption>();

        public ModEvent(bool isCountryEvent)
        {
            this.IsCountryEvent = isCountryEvent;
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "id": Id = parser.ReadString(); break;
                case "title": Title = parser.ReadString(); break;
                case "desc": Description = parser.ReadString(); break;
                case "picture": Picture = parser.ReadString(); break;
                case "is_triggered_only": IsTriggeredOnly = parser.ReadBool(); break;
                case "fire_only_once": FireOnlyOnce = parser.ReadBool(); break;
                case "hidden": Hidden = parser.ReadBool(); break;
                case "goto": GoTo = parser.ReadString(); break;

                case "mean_time_to_happen": MeanTimeToHappen = parser.Parse(new GroupNodeModel() { Name = "mean_time_to_happen" }); break;
                case "trigger": Trigger = parser.Parse(new GroupNodeModel() { Name = "trigger" }); break;
                case "immediate": Immediate = parser.Parse(new GroupNodeModel() { Name = "immediate" }); break;
                case "option": Options.Add(parser.Parse(new EventOption())); break;
                default: parser.Parse(new GroupNodeModel()); break;
            }
        }
    }
}
