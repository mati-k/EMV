using Caliburn.Micro;
using EMV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EMV.SharedData;
using GongSolutions.Wpf.DragDrop;
using EMV.Handlers;
using AutoCompleteTextBox.Editors;
using EMV.Models.Files;

namespace EMV.ViewModels
{
    public class MissionDetailsViewModel : Screen, IHandle<MissionFileModel>
    {
        private IEventAggregator _eventAggregator;
        private IWindowManager _windowManager;

        private MissionFileModel _missionFile;

        public MissionFileModel MissionFile
        {
            get { return _missionFile; }
            set
            {
                _missionFile = value;
                NotifyOfPropertyChange(() => MissionFile);
            }
        }

        public MissionDetailsViewModel(IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);

            _windowManager = windowManager;
        }

        public Task HandleAsync(MissionFileModel message, CancellationToken cancellationToken)
        {
            MissionFile = message;
            return Task.CompletedTask;
        }
    }
}
