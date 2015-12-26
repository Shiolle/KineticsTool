using System.Collections.ObjectModel;
using System.Linq;
using FireControl.Infrastructure.Interfaces;
using FireControl.Models.Interfaces;
using FireControl.Models.Interfaces.UnitControl;

namespace FireControl.ViewModels.UnitControl
{
    internal class UnitListViewModel : ViewModelBase
    {
        private readonly IUnitListModel _unitListModel;
        private readonly IUnitSetupModel _unitSetupModel;
        private readonly INavigationService _navigationService;

        private readonly ObservableCollection<UnitInfoPaneViewModel> _units;
        private UnitInfoPaneViewModel _selectedUnit;

        public UnitListViewModel(IUnitListModel unitListModel, IUnitSetupModel unitSetupModel, INavigationService navigationService)
        {
            _unitListModel = unitListModel;
            _unitSetupModel = unitSetupModel;
            _navigationService = navigationService;
            _units = new ObservableCollection<UnitInfoPaneViewModel>();
            _unitListModel.UnitsChanged += UnitListModelOnUnitsChanged;
            AddUnit = _navigationService.GetAddUnitRequest();
        }

        public ObservableCollection<UnitInfoPaneViewModel> Units
        {
            get { return _units; }
        }

        public UnitInfoPaneViewModel SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                OnPropertyChanged(Properties.SelectedUnit);
            }
        }

        public INavigationInterface AddUnit { get; private set; }

        public void DelCurrentUnit()
        {
            if (SelectedUnit != null && SelectedUnit.Model != null)
            {
                _unitListModel.RemoveUnit(SelectedUnit.Model);
            }
        }

        public void SaveList()
        {
            string filePath = _navigationService.GetSavePath();
            if (!string.IsNullOrEmpty(filePath))
            {
                _unitListModel.Save(filePath);
            }
        }

        public void LoadList()
        {
            string filePath = _navigationService.GetLoadPath();
            if (!string.IsNullOrEmpty(filePath))
            {
                _unitListModel.Load(filePath);
            }
        }

        private void UnitListModelOnUnitsChanged(ListAction action, IUnitModel affectedUnit)
        {
            switch (action)
            {
                case ListAction.Added:
                    _units.Add(new UnitInfoPaneViewModel(affectedUnit, _unitSetupModel, _navigationService));
                    return;
                case ListAction.Removed:
                    var affectedVm = _units.SingleOrDefault(un => un.Model == affectedUnit);
                    if (affectedUnit != null)
                    {
                        _units.Remove(affectedVm);
                    }
                    return;
                case ListAction.Reset:
                    _units.Clear();
                    _unitListModel.Models.ForEach(um => _units.Add(new UnitInfoPaneViewModel(um, _unitSetupModel, _navigationService)));
                    return;
            }
        }

        private static class Properties
        {
            public const string SelectedUnit = "SelectedUnit";
        }
    }
}
