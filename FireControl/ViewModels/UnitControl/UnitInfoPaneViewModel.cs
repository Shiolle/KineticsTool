using System;
using FireControl.Infrastructure.Interfaces;
using FireControl.Models.Interfaces;
using FireControl.Models.Interfaces.ShellStars;
using FireControl.Models.Interfaces.UnitControl;

namespace FireControl.ViewModels.UnitControl
{
    internal class UnitInfoPaneViewModel : ViewModelBase, IDisposable
    {
        private readonly IUnitSetupModel _unitSetupModel;
        private readonly IUnitModel _unitModel;

        public UnitInfoPaneViewModel(IUnitModel unitModel, IUnitSetupModel unitSetupModel, INavigationService navigationService)
        {
            if (unitModel == null)
            {
                throw new ArgumentNullException("unitModel");
            }
            _unitModel = unitModel;
            _unitSetupModel = unitSetupModel;
            ShowShellstars = navigationService.GetShellstarListRequest(AcquireShellstarListContext);
            SubscribeToEvents();
        }

        public IUnitModel Model
        {
            get { return _unitModel; }
        }

        public string Name
        {
            get { return _unitModel.Name; }
        }

        public string Position
        {
            get { return _unitModel.Position != null ? _unitModel.Position.ToString() : string.Empty; }
        }

        public bool CanViewShellstars
        {
            get { return _unitModel.AttachedShellstars != null && _unitModel.AttachedShellstars.Count > 0; }
        }

        public void SelectUnit(SelectionSlot slot)
        {
            _unitSetupModel.UpdateSelectedUnit(_unitModel, slot);
        }

        public INavigationInterface ShowShellstars { get; private set; }

        private object AcquireShellstarListContext()
        {
            return _unitModel;
        }

        private void SubscribeToEvents()
        {
            if (_unitModel != null)
            {
                _unitModel.Moved += UnitModelOnMoved;
                _unitModel.ShellstarListChanged += UnitModelOnShellstarListChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (_unitModel != null)
            {
                _unitModel.Moved -= UnitModelOnMoved;
                _unitModel.ShellstarListChanged -= UnitModelOnShellstarListChanged;
            }
        }

        private void UnitModelOnMoved()
        {
            OnPropertyChanged(Properties.Position);
        }

        private void UnitModelOnShellstarListChanged(ListAction action, IShellstarModel affectedShellstar)
        {
            OnPropertyChanged(Properties.CanViewShellstars);
        }

        public void Dispose()
        {
            UnsubscribeFromEvents();
        }

        private static class Properties
        {
            public const string Position = "Position";
            public const string CanViewShellstars = "CanViewShellstars";
        }
    }
}
