using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Models
{
    public class EventOption : GroupNodeModel
    {
        public override void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "name": Name = parser.ReadString(); break;
                default: base.TokenCallback(parser, token); break;
            }
        }
    }
}
