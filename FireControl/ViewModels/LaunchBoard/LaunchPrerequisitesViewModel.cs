using FireControl.Models.Interfaces.LaunchBoard;

namespace FireControl.ViewModels.LaunchBoard
{
    internal class LaunchPrerequisitesViewModel : ViewModelBase
    {
        private const string NoDataText = "N/A";

        private readonly WeaponSelectionViewModel _weaponSelection;
        private ILaunchBoardModel _launchBoardModel;

        public LaunchPrerequisitesViewModel(WeaponSelectionViewModel weaponSelection)
        {
            _weaponSelection = weaponSelection;
        }

        public string LaunchingUnitName
        {
            get { return _launchBoardModel != null ? _launchBoardModel.LaunchingUnit.Name : NoDataText; }
        }

        public string TargetUnitName
        {
            get { return _launchBoardModel != null ? _launchBoardModel.Target.Name : NoDataText; }
        }

        public string SegmentOfLaunch
        {
            get { return _launchBoardModel != null ? _launchBoardModel.LaunchingSegment.ToString() : NoDataText; }
        }

        public int CourseOffset
        {
            get { return _launchBoardModel != null ? _launchBoardModel.CourseOffset : 0; }
            set
            {
                if (_launchBoardModel == null)
                {
                    return;
                }
                _launchBoardModel.CourseOffset = value;
                OnPropertyChanged(Properties.CourseOffset);
            }
        }

        public WeaponSelectionViewModel WeaponSelection
        {
            get { return _weaponSelection; }
        }

        public void Initialize(ILaunchBoardModel launchBoard)
        {
            UnsubscribeFromEvents();
            _launchBoardModel = launchBoard;
            WeaponSelection.Initialize(_launchBoardModel.WeaponSelection);
            SubscribeToEvents();
            LaunchBoardModelOnVectorsUpdated();
        }

        public void CalculateFiringSolution()
        {
            if (_launchBoardModel != null)
            {
                _launchBoardModel.CalculateFiringSolution();
            }
        }

        private void LaunchBoardModelOnVectorsUpdated()
        {
            OnPropertyChanged(Properties.LaunchingUnitName);
            OnPropertyChanged(Properties.TargetUnitName);
            OnPropertyChanged(Properties.SegmentOfLaunch);
            OnPropertyChanged(Properties.CourseOffset);
        }

        private void SubscribeToEvents()
        {
            if (_launchBoardModel != null)
            {
                _launchBoardModel.VectorsUpdated += LaunchBoardModelOnVectorsUpdated;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (_launchBoardModel != null)
            {
                _launchBoardModel.VectorsUpdated -= LaunchBoardModelOnVectorsUpdated;
            }
        }

        private static class Properties
        {
            public const string LaunchingUnitName = "LaunchingUnitName";
            public const string TargetUnitName = "TargetUnitName";
            public const string SegmentOfLaunch = "SegmentOfLaunch";
            public const string CourseOffset = "CourseOffset";
        }
    }
}
