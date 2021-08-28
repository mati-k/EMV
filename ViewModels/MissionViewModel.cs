
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
    public class MissionViewModel : Conductor<object>.Collection.AllActive, IHandle<MissionModel>, IHandle<MissionBranchModel>
    {
        private IEventAggregator _eventAggregator;
        private MissionFileModel _missionFile;

        private MissionTreeViewModel _missionTreeViewModel;
        private MissionDetailsViewModel _missionDetailsViewModel;
        private BranchDetailsViewModel _branchDetailsViewModel;
        private Screen _selectedDetailsVM;
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

        public Screen SelectedDetailsVM
        {
            get { return _selectedDetailsVM; }
            set
            {
                _selectedDetailsVM = value;
                NotifyOfPropertyChange(() => SelectedDetailsVM);
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

        public MissionViewModel(IEventAggregator eventAggregator, MissionTreeViewModel missionTreeViewModel, MissionDetailsViewModel missionDetailsViewModel, BranchDetailsViewModel branchDetailsViewModel, IModData mod)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);

            _mod = mod;

            MissionTreeVM = missionTreeViewModel;
            _missionDetailsViewModel = missionDetailsViewModel;
            _branchDetailsViewModel = branchDetailsViewModel;

            ActivateItemAsync(MissionTreeVM);
        }

        public void SelectedMissionElementChanged(object selected)
        {
            if (selected is MissionBranchModel branch)
            {
                _eventAggregator.PublishOnUIThreadAsync(branch);
            }

            else if (selected is MissionModel mission)
            {
                _eventAggregator.PublishOnUIThreadAsync(mission);
            }
        }

        public void SelectTree(MissionFileModel missionFile)
        {
            _eventAggregator.PublishOnUIThreadAsync(missionFile);
        }

        public Task HandleAsync(MissionFileModel message, CancellationToken cancellationToken)
        {
            MissionFile = message;
            return Task.CompletedTask;
        }

        public Task HandleAsync(MissionModel message, CancellationToken cancellationToken)
        {
            if (_branchDetailsViewModel.IsActive)
                DeactivateItemAsync(_branchDetailsViewModel, false);

            SelectedDetailsVM = _missionDetailsViewModel;
            return ActivateItemAsync(_missionDetailsViewModel);
        }

        public Task HandleAsync(MissionBranchModel message, CancellationToken cancellationToken)
        {
            if (_missionDetailsViewModel.IsActive)
                DeactivateItemAsync(_missionDetailsViewModel, false);

            SelectedDetailsVM = _branchDetailsViewModel;
            return ActivateItemAsync(_branchDetailsViewModel);
        }
    }
}
