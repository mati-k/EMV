using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace EMV.SharedData
{
    public class StaticPaths : PropertyChangedBase
    {
        private static StaticPaths _instance;
        public static StaticPaths Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new StaticPaths();
                return _instance; 
            }
        }

        private string _missionFramePath;
        public string MissionFramePath
        {
            get { return _missionFramePath; }
            set
            {
                _missionFramePath = value;
                NotifyOfPropertyChange(() => MissionFramePath);
            }
        }

        private string _missionRewardPath;
        public string MissionRewardPath
        {
            get { return _missionRewardPath; }
            set
            {
                _missionRewardPath = value;
                NotifyOfPropertyChange(() => MissionRewardPath);
            }
        }

        private string _missionConditionPath;
        public string MissionConditionPath
        {
            get { return _missionConditionPath; }
            set
            {
                _missionConditionPath = value;
                NotifyOfPropertyChange(() => MissionConditionPath);
            }
        }

        private string _eventOptionPath;
        public string EventOptionPath
        {
            get { return _eventOptionPath; }
            set
            {
                _eventOptionPath = value;
                NotifyOfPropertyChange(() => EventOptionPath);
            }
        }
    }
}
