using Caliburn.Micro;
using EMV.Converters;
using EMV.Models;
using EMV.Views;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using EMV.SharedData;
using System.Windows.Media;
using EMV.Handlers;

namespace EMV.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<WindowMessage>
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();

        private IEventAggregator _eventAggregator;
        private IWindowManager _windowManager;

        private IModData _mod;

        public IModData Mod
        {
            get { return _mod; }
            set 
            {
                _mod = value;
                NotifyOfPropertyChange(() => Mod);
            }
        }

        public ShellViewModel(IEventAggregator eventAggregator, IWindowManager windowManager, IModData mod)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);

            _mod = mod;

            ActivateItemAsync(IoC.GetInstance(typeof(StartViewModel), null), CancellationToken.None);
        }

        public Task HandleAsync(WindowMessage message, CancellationToken cancellationToken)
        {
            return ActivateItemAsync(IoC.GetInstance(message.ViewModelType, null));
        }

        public void GoToMenu()
        {
            ActivateItemAsync(IoC.GetInstance(typeof(MenuViewModel), null));
        }
    }
}
