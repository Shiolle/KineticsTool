using FireControl.Infrastructure.Interfaces;
using FireControl.Models.Interfaces;
using FireControl.Models.Interfaces.ShellStars;
using FireControl.Models.Interfaces.UnitControl;
using FireControl.ViewModels.Avid;
using FireControl.ViewModels.DataContexts;

namespace FireControl.ViewModels.UnitControl
{
    internal class SelectedUnitViewModel : ViewModelBase
    {
        private const string ShellstarCountTemplate = "({0})";

        private readonly IUnitSetupModel _unitSetupModel;

        public SelectedUnitViewModel(IUnitSetupModel unitSetupModel,
                                     PositionControlViewModel positionControlViewModel,
                                     VectorsControlViewModel vectorsControlViewModel,
                                     INavigationService navigationService)
        {
            _unitSetupModel = unitSetupModel;
            _unitSetupModel.SelectionChanged += UnitSetupModelOnSelectionChanged;

            PositionViewModel = positionControlViewModel;
            VectorsViewModel = vectorsControlViewModel;

            LaunchSeekers = navigationService.GetLaunchBoardRequest(AcquireLaunchBoardContext);
            ViewShellstars = navigationService.GetShellstarListRequest(AcquireShellstarListContext);
            UpdateShellstarInfo();
            SubscribeToEvents();
        }

        public SelectionSlot Slot { get; set; }

        private const string NoUnitSelectedMessage = "N/A";

        private IUnitModel _selectedUnit;

        public string UnitName
        {
            get { return _selectedUnit != null ? _selectedUnit.Name : NoUnitSelectedMessage; }
        }

        public bool IsUnitSelected
        {
            get { return _selectedUnit != null; }
        }

        public bool CanFire
        {
            get { return _unitSetupModel.BothUnitsSelected; }
        }

        public bool CanViewShellstars
        {
            get { return _selectedUnit != null && _selectedUnit.AttachedShellstars.Count > 0; }
        }

        public string ShellstarCount
        {
            get { return _selectedUnit != null ? string.Format(ShellstarCountTemplate, _selectedUnit.AttachedShellstars.Count) : "(0)"; }
        }

        public PositionControlViewModel PositionViewModel { get; private set; }

        public VectorsControlViewModel VectorsViewModel { get; private set; }

        public INavigationInterface LaunchSeekers { get; private set; }

        public INavigationInterface ViewShellstars { get; private set; }

        private object AcquireLaunchBoardContext()
        {
            return new LaunchBoardDataContext
            {
                Target = _unitSetupModel.GetPair(Slot),
                Attacker = _selectedUnit
            };
        }

        private object AcquireShellstarListContext()
        {
            return _selectedUnit;
        }

        private void OnNewUnitSelected()
        {
            OnPropertyChanged(Properties.UnitName);
            OnPropertyChanged(Properties.IsUnitSelected);
            OnPropertyChanged(Properties.CanFire);
        }

        private void UnitSetupModelOnSelectionChanged(SelectionSlot slot, IUnitModel newUnit)
        {
            if (slot == Slot)
            {
                UnsubscribeFromEvents();
                _selectedUnit = newUnit;
                PositionViewModel.SelectUnit(_selectedUnit);
                VectorsViewModel.SelectUnit(_selectedUnit);
                SubscribeToEvents();
                OnNewUnitSelected();
                UpdateShellstarInfo();
            }
            else
            {
                OnPropertyChanged(Properties.CanFire);
            }
        }

        private void UpdateShellstarInfo()
        {
            OnPropertyChanged(Properties.CanViewShellstars);
            OnPropertyChanged(Properties.ShellstarCount);
        }

        private void SubscribeToEvents()
        {
            if (_selectedUnit != null)
            {
                _selectedUnit.ShellstarListChanged += SelectedUnitOnShellstarListChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (_selectedUnit != null)
            {
                _selectedUnit.ShellstarListChanged -= SelectedUnitOnShellstarListChanged;
            }
        }

        private void SelectedUnitOnShellstarListChanged(ListAction action, IShellstarModel affectedShellstar)
        {
            UpdateShellstarInfo();
        }

        private static class Properties
        {
            public const string UnitName = "UnitName";
            public const string IsUnitSelected = "IsUnitSelected";
            public const string CanFire = "CanFire";
            public const string CanViewShellstars = "CanViewShellstars";
            public const string ShellstarCount = "ShellstarCount";
        }
    }
}
