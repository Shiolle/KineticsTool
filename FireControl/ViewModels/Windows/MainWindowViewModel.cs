using FireControl.Models.Interfaces.UnitControl;
using FireControl.ViewModels.Misc;
using FireControl.ViewModels.TimeControl;
using FireControl.ViewModels.UnitControl;

namespace FireControl.ViewModels.Windows
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly TurnControlViewModel _turnControlViewModel;
        private readonly UnitListViewModel _unitListViewModel;
        private readonly SelectedUnitViewModel _upperSelectedUnitViewModel;
        private readonly SelectedUnitViewModel _lowerSelectedUnitViewModel;
        private readonly LogoPanelViewModel _logoPanelViewModel;

        public MainWindowViewModel(TurnControlViewModel turnControlViewModel,
                                   UnitListViewModel unitListViewModel,
                                   SelectedUnitViewModel upperSelectedUnitViewModel,
                                   SelectedUnitViewModel lowerSelectedUnitViewModel,
                                   LogoPanelViewModel logoPanelViewModel)
        {
            _turnControlViewModel = turnControlViewModel;
            _unitListViewModel = unitListViewModel;
            _upperSelectedUnitViewModel = upperSelectedUnitViewModel;
            _lowerSelectedUnitViewModel = lowerSelectedUnitViewModel;
            _logoPanelViewModel = logoPanelViewModel;

            _upperSelectedUnitViewModel.Slot = SelectionSlot.FirstUnit;
            _lowerSelectedUnitViewModel.Slot = SelectionSlot.SecondUnit;
        }

        public TurnControlViewModel TurnControlViewModel
        {
            get { return _turnControlViewModel; }
        }

        public UnitListViewModel UnitListViewModel
        {
            get { return _unitListViewModel; }
        }

        public SelectedUnitViewModel UpperSelection
        {
            get { return _upperSelectedUnitViewModel; }
        }

        public SelectedUnitViewModel LowerSelection
        {
            get { return _lowerSelectedUnitViewModel; }
        }

        public LogoPanelViewModel LogoPanel
        {
            get { return _logoPanelViewModel; }
        }

        public bool BothUnitsSelected
        {
            get { return _upperSelectedUnitViewModel.IsUnitSelected && _lowerSelectedUnitViewModel.IsUnitSelected; }
        }
    }
}
