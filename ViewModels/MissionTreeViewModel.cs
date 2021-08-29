using Caliburn.Micro;
using EMV.Models;
using EMV.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Specialized;
using EMV.Models.Files;
using System.ComponentModel;

namespace EMV.ViewModels
{
    public class MissionTreeViewModel : Screen, IHandle<MissionFileModel>
    {
        private IEventAggregator _eventAggregator;
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

        public MissionTreeViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
        }

        public Task HandleAsync(MissionFileModel message, CancellationToken cancellationToken)
        {
            MissionFile = message;

            foreach (Flag flag in MissionFile.Flags)
            {
                flag.PropertyChanged += FlagChanged;
            }

            MissionTreeChanged();
            return Task.CompletedTask;
        }

        private void FlagChanged(object sender, PropertyChangedEventArgs e)
        {
            MissionTreeChanged();
        }

        private void MissionTreeChanged()
        {
            (GetView() as MissionTreeView).UpdateMissionTree();
        }

        public void SelectMission(MissionModel mission)
        {
            _eventAggregator.PublishOnUIThreadAsync(mission);
        }
    }
}