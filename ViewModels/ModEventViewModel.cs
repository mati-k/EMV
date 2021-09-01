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
        private BindableCollection<ModEvent> _selectedEvents;
        private ModEvent _selectedEvent;
        private string _nameQuery;

        public BindableCollection<ModEvent> AllEvents { get; set; }


        public IModData Mod
        {
            get { return _mod; }
            set
            {
                _mod = value;
                NotifyOfPropertyChange(() => Mod);
            }
        }

        public BindableCollection<ModEvent> SelectedEvents
        {
            get { return _selectedEvents; }
            set
            {
                _selectedEvents = value;
                NotifyOfPropertyChange(() => SelectedEvents);
            }
        }

        public ModEvent SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                _selectedEvent = value;
                NotifyOfPropertyChange(() => SelectedEvent);
            }
        }

        public string NameQuery
        {
            get { return _nameQuery; }
            set
            {
                _nameQuery = value;
                NotifyOfPropertyChange(() => NameQuery);
                UpdateSelection();
            }
        }

        public ModEventViewModel(IModData mod)
        {
            this._mod = mod;
            this.AllEvents = new BindableCollection<ModEvent>(mod.EventFiles.SelectMany(file => file.Events));
        }

        public void SelectFile(EventFile eventFile)
        {
            SelectedEvents = eventFile.Events;
        }
        public void SelectEvent(ModEvent modEvent)
        {
            SelectedEvent = modEvent;
        }

        private void UpdateSelection()
        {
            if (String.IsNullOrWhiteSpace(NameQuery))
                SelectedEvents = AllEvents;
            else
                SelectedEvents = new BindableCollection<ModEvent>(AllEvents.Where(
                    m => m.Title.Contains(NameQuery, StringComparison.OrdinalIgnoreCase) || 
                    m.Id.Contains(NameQuery, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
