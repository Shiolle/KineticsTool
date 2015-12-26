using System;
using System.Collections.ObjectModel;
using System.Linq;
using FireControl.Models.Implementation.ShellStars;
using FireControl.Models.Interfaces.ShellStars;

namespace FireControl.ViewModels.Shellstar
{
    internal class ShellstarInfoViewModel : ViewModelBase
    {
        private const int ItemsPerRow = 6;
        private const string OverflowMessageTemplate = "... +{0}\r\nImpulses";

        private IShellstarModel _shellstarModel;
        private string _targetName;
        private bool _impulseOverflow;
        private string _overflowMessage;

        public ShellstarInfoViewModel()
        {
            ImpulseTrack = new ObservableCollection<ImpulseTrackElementModel>[3];

            for (int i = 0; i < 3; i++)
            {
                ImpulseTrack[i] = new ObservableCollection<ImpulseTrackElementModel>();
            }
    }

        public void Initialize(IShellstarModel shellstar, string targetName)
        {
            UnsubscribeFromEvents();
            _shellstarModel = shellstar;
            TargetName = targetName;
            ResetImpulseTrack();
            OnNewShellstar();
            SubscribeToEvents();
        }

        public bool IsDataAvailable
        {
            get { return _shellstarModel != null; }
        }

        public string TargetName
        {
            get { return _targetName; }
            private set
            {
                _targetName = value;
                OnPropertyChanged(Properties.TargetName);
            }
        }

        public float RoC
        {
            get { return _shellstarModel != null ? _shellstarModel.Shellstar.RoC : 0; }
            
        }

        public int Dmg50
        {
            get { return _shellstarModel != null ? _shellstarModel.Shellstar.Dmg50 : 0; }
        }

        public int Dmg100
        {
            get { return _shellstarModel != null ? _shellstarModel.Shellstar.Dmg100 : 0; }
        }

        public int Dmg200
        {
            get { return _shellstarModel != null ? _shellstarModel.Shellstar.Dmg200 : 0; }
        }

        public bool ImpulseOverflow
        {
            get { return _impulseOverflow; }
            set
            {
                _impulseOverflow = value;
                OnPropertyChanged(Properties.ImpulseOverflow);
            }
        }

        public string ImpulseOverflowMessage
        {
            get { return _overflowMessage; }
            set
            {
                _overflowMessage = value;
                OnPropertyChanged(Properties.ImpulseOverflowMessage);
            }
        }

        public string ImpactWindow
        {
            get { return _shellstarModel != null && _shellstarModel.EvasionInfo.DirectionsDefined ? _shellstarModel.EvasionInfo.ImpactWindow.ToString() : string.Empty; }
        }

        public string EvasionU
        {
            get { return _shellstarModel != null && _shellstarModel.EvasionInfo.DirectionsDefined ? _shellstarModel.EvasionInfo.EvasionUp.ToString() : string.Empty; }
        }

        public string EvasionD
        {
            get { return _shellstarModel != null && _shellstarModel.EvasionInfo.DirectionsDefined ? _shellstarModel.EvasionInfo.EvasionDown.ToString() : string.Empty; }
        }

        public string EvasionL
        {
            get { return _shellstarModel != null && _shellstarModel.EvasionInfo.DirectionsDefined ? _shellstarModel.EvasionInfo.EvasionLeft.ToString() : string.Empty; }
        }

        public string EvasionR
        {
            get { return _shellstarModel != null && _shellstarModel.EvasionInfo.DirectionsDefined ? _shellstarModel.EvasionInfo.EvasionRight.ToString() : string.Empty; }
        }

        // Unfortunately WPF stack panels always fill bottom row last, regardless of alignments settings,
        // so I had to separate impulse track into rows manually.
        public ObservableCollection<ImpulseTrackElementModel>[] ImpulseTrack { get; private set; }

        private void ResetImpulseTrack()
        {
            foreach (var trackRow in ImpulseTrack)
            {
                trackRow.Clear();
            }
            int trackLength = _shellstarModel != null ? _shellstarModel.Shellstar.ImpulseTrack.Count : 0;

            ImpulseOverflow = trackLength > ItemsPerRow * 3;
            ImpulseOverflowMessage = ImpulseOverflow ? string.Format(OverflowMessageTemplate, Math.Abs(ItemsPerRow * 3 - trackLength)) : string.Empty;

            if (_shellstarModel == null || _shellstarModel.Shellstar.ImpulseTrack == null || _shellstarModel.Shellstar.ImpulseTrack.Count == 0)
            {
                return;
            }

            for (int i = 0; i < 3; i++)
            {
                int itemsOnThisRow = Math.Min(trackLength, (i + 1) * ItemsPerRow) - i * ItemsPerRow;
                if (itemsOnThisRow > 0) //This row of the track is not empty.
                {
                    var rowImpulses = _shellstarModel.Shellstar.ImpulseTrack.GetRange(trackLength - itemsOnThisRow - i * ItemsPerRow, itemsOnThisRow);
                    rowImpulses.ForEach(ite => ImpulseTrack[i].Add(new ImpulseTrackElementModel(ite)));
                }
            }

            // Hide last element
            if (trackLength > 0)
            {
                ImpulseTrack[0].Last().IsVisible = false;
            }
        }

        private void SubscribeToEvents()
        {
            if (_shellstarModel != null)
            {
                _shellstarModel.EvasionInfo.EvasionDirectionsChanged += OnEvasionChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (_shellstarModel != null)
            {
                _shellstarModel.EvasionInfo.EvasionDirectionsChanged -= OnEvasionChanged;
            }
        }

        private void OnEvasionChanged()
        {
            OnPropertyChanged(Properties.ImpactWindow);
            OnPropertyChanged(Properties.EvasionU);
            OnPropertyChanged(Properties.EvasionD);
            OnPropertyChanged(Properties.EvasionL);
            OnPropertyChanged(Properties.EvasionR);
        }

        private void OnNewShellstar()
        {
            OnPropertyChanged(Properties.RoC);
            OnPropertyChanged(Properties.IsDataAvailable);
            OnPropertyChanged(Properties.Dmg50);
            OnPropertyChanged(Properties.Dmg100);
            OnPropertyChanged(Properties.Dmg200);
            OnEvasionChanged();
        }

        private static class Properties
        {
            public const string ImpulseOverflow = "ImpulseOverflow";
            public const string ImpulseOverflowMessage = "ImpulseOverflowMessage";
            public const string TargetName = "TargetName";
            public const string RoC = "RoC";
            public const string IsDataAvailable = "IsDataAvailable";
            public const string Dmg50 = "Dmg50";
            public const string Dmg100 = "Dmg100";
            public const string Dmg200 = "Dmg200";
            public const string ImpactWindow = "ImpactWindow";
            public const string EvasionU = "EvasionU";
            public const string EvasionD = "EvasionD";
            public const string EvasionL = "EvasionL";
            public const string EvasionR = "EvasionR";
        }
    }
}
