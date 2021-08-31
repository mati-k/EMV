using Caliburn.Micro;
using EMV.Models;
using EMV.Models.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.ViewModels
{
    public class ModEventViewModel : Screen
    {
        private IModData _mod;
        private EventFile _selectedEventFile;

        public IModData Mod
        {
            get { return _mod; }
            set
            {
                _mod = value;
                NotifyOfPropertyChange(() => Mod);
            }
        }

        public EventFile SelectedEventFile
        {
            get { return _selectedEventFile; }
            set
            {
                _selectedEventFile = value;
                NotifyOfPropertyChange(() => SelectedEventFile);
            }
        }

        public ModEventViewModel(IModData mod)
        {
            this._mod = mod;
        }

        public void SelectFile(EventFile eventFile)
        {
            SelectedEventFile = eventFile;
        }
    }
}
