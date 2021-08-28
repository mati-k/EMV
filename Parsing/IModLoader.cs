using EMV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Parsing
{
    public interface IModLoader
    {
        public void LoadModData(IModData mod, FilesModel files);
    }
}
