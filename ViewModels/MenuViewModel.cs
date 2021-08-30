using Caliburn.Micro;
using EMV.Handlers;
using EMV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMV.ViewModels
{
    public class MenuViewModel : Screen, IHandle<ModData>
    {
        private IEventAggregator _eventAggregator;

        public MenuViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
        }

        public void Missions()
        {
            _eventAggregator.PublishOnUIThreadAsync(new WindowMessage(typeof(MissionViewModel)));
        }

        public void Modifiers()
        {
            _eventAggregator.PublishOnUIThreadAsync(new WindowMessage(typeof(ModifierViewModel)));
        }

        public Task HandleAsync(ModData message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
