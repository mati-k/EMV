using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Exceptions
{
    public class BranchNameException : Exception
    {
        public BranchNameException()
            : base("Branch without name")
        { }
    }
}
