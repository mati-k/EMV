using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Models
{
    public class Modifier : GroupNodeModel
    {
        private string _titile;
        
        public string Title
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_titile))
                    return Name;
                return _titile;
            }

            set
            {
                _titile = value;
                NotifyOfPropertyChange(() => Title);
            }
        }
        public string Description { get; set; } = "";
    }
}
