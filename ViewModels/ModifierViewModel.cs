using Caliburn.Micro;
using EMV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.ViewModels
{
    public class ModifierViewModel : Screen
    {
        private IModData _mod;
        private string _nameQuery;

        private BindableCollection<Modifier> _selectedModifiers;
        public BindableCollection<Modifier> AllModifiers { get; set; }
        public BindableCollection<Modifier> SelectedModifiers
        {
            get { return _selectedModifiers; }
            set
            {
                _selectedModifiers = value;
                NotifyOfPropertyChange(() => SelectedModifiers);
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

        public ModifierViewModel(IModData mod)
        {
            this._mod = mod;
            AllModifiers = new BindableCollection<Modifier>(_mod.ModifierFiles.SelectMany(file => file.Modifiers).ToList());
            SelectedModifiers = AllModifiers;
        }

        private void UpdateSelection()
        {
            if (String.IsNullOrWhiteSpace(NameQuery))
                SelectedModifiers = AllModifiers;
            else
                SelectedModifiers = new BindableCollection<Modifier>(AllModifiers.Where(m => m.Title.Contains(NameQuery, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
