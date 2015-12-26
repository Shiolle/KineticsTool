using FireControl.Infrastructure.Interfaces;
using FireControl.Models.Interfaces.LaunchBoard;
using FireControl.ViewModels.DataContexts;
using Kinetics.Core.Data.FiringSolution;
using Kinetics.Core.Data.RefData;

namespace FireControl.ViewModels.LaunchBoard
{
    internal class RoCWorksheetViewModel : ViewModelBase
    {
        public RoCWorksheetViewModel(INavigationService navigationService)
        {
            ViewShellstar = navigationService.GetShellstarDetailsRequest(AcquireShellstarContext);
        }

        private ILaunchBoardModel _launchBoardModel;
        private FiringSolution _firingSolution;

        public bool IsDataAvailable
        {
            get { return _firingSolution != null && !NoShot; }
        }

        public int CrossingVector
        {
            get { return _firingSolution != null ? _firingSolution.CrossingVector : 0; }
        }

        public int MuzzleVelocity
        {
            get { return _firingSolution != null ? _firingSolution.MuzzleVelocity : 0; }
        }

        public float CvAdjustment
        {
            get { return _firingSolution != null ? _firingSolution.CrossingVectorAdjustment : 0; }
        }

        public float MvAdjustment
        {
            get { return _firingSolution != null ? _firingSolution.MuzzleVelocityAdjustment : 0; }
        }

        public int ModifiedCv
        {
            get { return _firingSolution != null ? _firingSolution.ModifiedCrossingVector : 0; }
        }

        public int ModifiedMv
        {
            get { return _firingSolution != null ? _firingSolution.ModifiedMuzzleVelocity : 0; }
        }

        public int RoCTurn
        {
            get { return _firingSolution != null ? _firingSolution.RoCTurn : 0; }
        }

        public float RoC
        {
            get { return _firingSolution != null ? _firingSolution.RoC : 0; }
        }

        public bool CanViewShellstar
        {
            get
            {
                return _launchBoardModel != null && _launchBoardModel.Shellstar != null;
            }
        }

        public bool NoShot
        {
            get { return _launchBoardModel != null && _firingSolution != null && _firingSolution.AimAdjustment == AimAdjustment.NoShot; }
        }

        public string NoShotComment
        {
            get { return _firingSolution != null ? string.Format("No Shot (RoC {0})", _firingSolution.RoC) : string.Empty; }
        }

        public INavigationInterface ViewShellstar { get; private set; }

        public void Initialize(ILaunchBoardModel launchBoardModel)
        {
            UnsubscribeFromEvents();
            _launchBoardModel = launchBoardModel;
            SubscribeToEvents();
            LaunchBoardModelOnFiringSolutionUpdated();
        }

        private object AcquireShellstarContext()
        {
            return new ShellstarDetailsDataContext
            {
                LaunchWindows = _launchBoardModel.LaunchWindows,
                Shellstar = _launchBoardModel.Shellstar,
                Target = _launchBoardModel.Target
            };
        }

        private void SubscribeToEvents()
        {
            if (_launchBoardModel != null)
            {
                _launchBoardModel.FiringSolutionUpdated += LaunchBoardModelOnFiringSolutionUpdated;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (_launchBoardModel != null)
            {
                _launchBoardModel.FiringSolutionUpdated -= LaunchBoardModelOnFiringSolutionUpdated;
            }
        }

        private void LaunchBoardModelOnFiringSolutionUpdated()
        {
            _firingSolution = _launchBoardModel.FiringSolution;
            OnPropertyChanged(Properties.CanViewShellstar);
            OnPropertyChanged(Properties.IsDataAvailable);
            OnPropertyChanged(Properties.CrossingVector);
            OnPropertyChanged(Properties.MuzzleVelocity);
            OnPropertyChanged(Properties.NoShotComment);
            OnPropertyChanged(Properties.CvAdjustment);
            OnPropertyChanged(Properties.MvAdjustment);
            OnPropertyChanged(Properties.ModifiedCv);
            OnPropertyChanged(Properties.ModifiedMv);
            OnPropertyChanged(Properties.RoCTurn);
            OnPropertyChanged(Properties.NoShot);
            OnPropertyChanged(Properties.RoC);
        }

        private static class Properties
        {
            public const string CanViewShellstar = "CanViewShellstar";
            public const string IsDataAvailable = "IsDataAvailable";
            public const string CrossingVector = "CrossingVector";
            public const string MuzzleVelocity = "MuzzleVelocity";
            public const string NoShotComment = "NoShotComment";
            public const string CvAdjustment = "CvAdjustment";
            public const string MvAdjustment = "MvAdjustment";
            public const string ModifiedCv = "ModifiedCv";
            public const string ModifiedMv = "ModifiedMv";
            public const string RoCTurn = "RoCTurn";
            public const string NoShot = "NoShot";
            public const string RoC = "RoC";
        }
    }
}
