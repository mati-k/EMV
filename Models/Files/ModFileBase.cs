using Caliburn.Micro;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Models.Files
{
    public abstract class ModFileBase : PropertyChangedBase, IParadoxRead
    {
        public string FileName { get; set; }

        public abstract void TokenCallback(ParadoxParser parser, string token);
        public abstract void BindLocalisation(Dictionary<string, string> localisation);
    }
}
