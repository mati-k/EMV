using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Handlers
{
    public class WindowMessage
    {
        public Type ViewModelType { get; private set; }

        public WindowMessage(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

    }
}
