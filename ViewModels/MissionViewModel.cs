
using Caliburn.Micro;
using EMV.Handlers;
using EMV.Models;
using EMV.Models.Files;
using EMV.Views;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMV.ViewModels
{
    public class MissionViewModel : Conductor<object>.Collection.AllActive
    {
        private IEventAggregator _eventAggregator;
        private MissionFileModel _missionFile;

        private MissionTreeViewModel _missionTreeViewModel;
        private MissionDetailsViewModel _missionDetailsViewModel;

        private IModData _mod;

        public IDropTarget DropHandler { get; } = new DropTargetHandler();

        public MissionTreeViewModel MissionTreeVM
        {
            get { return _missionTreeViewModel; }
            set
            {
                _missionTreeViewModel = value;
                NotifyOfPropertyChange(() => MissionTreeVM);
            }
        }

        public MissionFileModel MissionFile 
        {
            get { return _missionFile; }
            set
            {
                _missionFile = value;
                NotifyOfPropertyChange(() => MissionFile);
            }
        }

        public MissionDetailsViewModel MissionDetailsVM
        {
            get { return _missionDetailsViewModel; }
            set
            {
                _missionDetailsViewModel = value;
                NotifyOfPropertyChange(() => MissionDetailsVM);
            }
        }

        public IModData Mod
        {
            get { return _mod; }
            set
            {
                _mod = value;
                NotifyOfPropertyChange(() => Mod);
            }
        }

        public MissionViewModel(IEventAggregator eventAggregator, MissionTreeViewModel missionTreeViewModel, MissionDetailsViewModel missionDetailsViewModel, IModData mod)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);

            _mod = mod;

            MissionTreeVM = missionTreeViewModel;
            MissionDetailsVM = missionDetailsViewModel;

            ActivateItemAsync(MissionTreeVM);
            ActivateItemAsync(MissionDetailsVM);
        }

        public void SelectTree(MissionFileModel missionFile)
        {
            MissionFile = missionFile;
            _eventAggregator.PublishOnUIThreadAsync(missionFile);
        }
    }
}
