using System.Collections.ObjectModel;
using FireControl.Models.Implementation.LaunchBoard;
using FireControl.Models.Interfaces.LaunchBoard;
using Kinetics.Core.Data.FiringSolution;

namespace FireControl.ViewModels.LaunchBoard
{
    internal class MissileAccelerationViewModel : ViewModelBase
    {
        public MissileAccelerationViewModel()
        {
            AccelerationTrack = new ObservableCollection<IAccelerationImpulse>();
        }

        private ILaunchBoardModel _launchBoardModel;
        private MissileAccelerationData _accelerationData;

        public void Initialize(ILaunchBoardModel launchBoardModel)
        {
            UnsubscribeFromEvents();
            _launchBoardModel = launchBoardModel;
            SubscribeToEvents();
            LaunchBoardModelOnFiringSolutionUpdated();
        }

        public bool IsDataAvailable
        {
            get { return _accelerationData != null && _accelerationData.ValidLaunch; }
        }

        public bool IsLaunchInvalid
        {
            get { return _accelerationData != null && !_accelerationData.ValidLaunch; }
        }

        public float RoC
        {
            get { return _accelerationData != null ? _launchBoardModel.FiringSolution.RoC : 0; }
        }

        public int BurnDuration
        {
            get { return _accelerationData != null ? _accelerationData.BurnDuration : 0; }
        }

        public float TotalAcceleration
        {
            get { return _accelerationData != null ? _accelerationData.TotalAcceleration : 0; }
        }

        public float MpatTotal
        {
            get { return _accelerationData != null ? _accelerationData.TotalPositionAdjustment : 0; }
        }

        public float BurnDistance
        {
            get { return _accelerationData != null ? _accelerationData.BurnDistance : 0; }
        }

        public float TargetRange
        {
            get { return _accelerationData != null ? _accelerationData.TargetRange : 0; }
        }

        public ObservableCollection<IAccelerationImpulse> AccelerationTrack { get; private set; }

        private void ResetAccelerationTrack()
        {
            AccelerationTrack.Clear();
            if (_accelerationData == null || _accelerationData.ImpulseData == null)
            {
                return;
            }
            float roc = RoC;
            _accelerationData.ImpulseData.ForEach(mai => AccelerationTrack.Add(new AccelerationImpulse(mai, roc)));
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
            _accelerationData = _launchBoardModel.AccelerationData;
            OnPropertyChanged(Properties.IsDataAvailable);
            OnPropertyChanged(Properties.IsLaunchInvalid);
            OnPropertyChanged(Properties.RoC);
            OnPropertyChanged(Properties.BurnDuration);
            OnPropertyChanged(Properties.TotalAcceleration);
            OnPropertyChanged(Properties.MpatTotal);
            OnPropertyChanged(Properties.BurnDistance);
            OnPropertyChanged(Properties.TargetRange);
            ResetAccelerationTrack();
        }

        private static class Properties
        {
            public const string IsDataAvailable = "IsDataAvailable";
            public const string IsLaunchInvalid = "IsLaunchInvalid";
            public const string RoC = "RoC";
            public const string BurnDuration = "BurnDuration";
            public const string TotalAcceleration = "TotalAcceleration";
            public const string MpatTotal = "MpatTotal";
            public const string BurnDistance = "BurnDistance";
            public const string TargetRange = "TargetRange";
        }

    }
}
