using System.Linq;
using FireControl.Infrastructure.Interfaces;
using FireControl.Models.Interfaces.ShellStars;
using FireControl.Models.Interfaces.UnitControl;
using FireControl.ViewModels.DataContexts;
using Kinetics.Core.Data.Avid;

namespace FireControl.ViewModels.Shellstar
{
    internal class ShellstarDetailsViewModel : ViewModelBase, INavigationNode
    {
        private const string CaptionFormat = "-> {0} at {1}";

        private IUnitModel _target;
        private IShellstarModel _shellstar;

        private readonly ShellstarInfoViewModel _shellstarViewModel;
        private readonly LaunchWindowControlViewModel _launchWindowControlViewModel;

        private bool _isDataAvailable;
        private bool _canAttach;
        private string _caption;

        public ShellstarDetailsViewModel(ShellstarInfoViewModel shellstarViewModel, LaunchWindowControlViewModel launchWindowControlViewModel)
        {
            _shellstarViewModel = shellstarViewModel;

            _launchWindowControlViewModel = launchWindowControlViewModel;
        }

        public void Initialize(object dataContext)
        {
            var context = VerifyContext<ShellstarDetailsDataContext>(dataContext);

            if (context != null)
            {
                Initialize(context.Target, context.Shellstar, context.LaunchWindows);
            }
        }

        public void Initialize(IUnitModel target, IShellstarModel shellstar, AvidWindow[] launchWindows)
        {
            UnsubscribeFromEvents();
            _target = target;
            _shellstar = shellstar;
            CanAttach = GetCanAttachShellstar();
            Caption = GetCaption();
            IsDataAvailable = GetIsDataAvailable();

            string targetName = _target != null ? _target.Name : string.Empty;
            _shellstarViewModel.Initialize(shellstar, targetName);
            _launchWindowControlViewModel.Initialize(_shellstar != null ? _shellstar.EvasionInfo : null, launchWindows);
            SubscribeToEvents();
        }

        public bool IsDataAvailable
        {
            get { return _isDataAvailable; }
            private set
            {
                _isDataAvailable = value;
                OnPropertyChanged(Properties.IsDataAvailable);
            }
        }

        public ShellstarInfoViewModel ShellStarViewModel
        {
            get { return _shellstarViewModel; }
        }

        public LaunchWindowControlViewModel LaunchWindowControlViewModel
        {
            get { return _launchWindowControlViewModel; }
        }

        public bool CanAttach
        {
            get { return _canAttach; }
            private set
            {
                _canAttach = value;
                OnPropertyChanged(Properties.CanAttach);
            }
        }

        public string Caption
        {
            get { return _caption; }
            private set
            {
                _caption = value;
                OnPropertyChanged(Properties.Caption);
            }
        }

        public string Tag
        {
            get { return _shellstar != null ? _shellstar.Tag : string.Empty; }
            set
            {
                if (_shellstar != null)
                {
                    _shellstar.Tag = value;
                    OnPropertyChanged(Properties.Tag);
                }
            }
        }

        public bool NeedReferenceDirection
        {
            get { return _shellstar != null && !_shellstar.EvasionInfo.DirectionsDefined; }
        }

        public void AttachShellstar()
        {
            if (_target == null || _shellstar == null)
            {
                return;
            }

            _target.AttachShellstar(_shellstar);
        }

        private bool GetCanAttachShellstar()
        {
            if (_shellstar == null || _target == null)
            {
                return false;
            }
            return !_target.AttachedShellstars.Contains(_shellstar) && _shellstar.EvasionInfo.DirectionsDefined;
        }

        private string GetCaption()
        {
            if (_shellstar == null || _target == null)
            {
                return string.Empty;
            }

            return string.Format(CaptionFormat, _target.Name, _shellstar.TimeOfLaunch);
        }

        private bool GetIsDataAvailable()
        {
            return _shellstar != null && _target != null;
        }

        private void EvasionInfoOnEvasionDirectionsChanged()
        {
            CanAttach = GetCanAttachShellstar();
            OnPropertyChanged(Properties.NeedReferenceDirection);
        }

        private void SubscribeToEvents()
        {
            if (_shellstar != null)
            {
                _shellstar.EvasionInfo.EvasionDirectionsChanged += EvasionInfoOnEvasionDirectionsChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (_shellstar != null)
            {
                _shellstar.EvasionInfo.EvasionDirectionsChanged -= EvasionInfoOnEvasionDirectionsChanged;
            }
        }

        private static class Properties
        {
            public const string IsDataAvailable = "IsDataAvailable";
            public const string Caption = "Caption";
            public const string CanAttach = "CanAttach";
            public const string Tag = "Tag";
            public const string NeedReferenceDirection = "NeedReferenceDirection";
        }
    }
}
