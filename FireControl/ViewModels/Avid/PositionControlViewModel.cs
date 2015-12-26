using System;
using FireControl.Models.Interfaces.UnitControl;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;

namespace FireControl.ViewModels.Avid
{
    internal class PositionControlViewModel : ViewModelBase
    {
        private IUnitModel _selectedUnit;

        public SelectionSlot Slot { get; set; }

        public string Position
        {
            get { return _selectedUnit != null && _selectedUnit.Position != null ? _selectedUnit.Position.ToString() : string.Empty; }
            set
            {
                _selectedUnit.Position = HexGridCoordinate.Parse(value);
                OnPropertyChanged(Properties.Position);
            }
        }

        public void SelectUnit(IUnitModel unit)
        {
            _selectedUnit = unit;
            OnNewUnitSelected();
        }

        public void MoveUnit(string direction)
        {
            var axis = (HexAxis)Enum.Parse(typeof(HexAxis), direction);

            if (_selectedUnit == null)
            {
                throw new InvalidOperationException("Can't move unit before it is selected.");
            }

            _selectedUnit.Move(axis);
            OnMovement();
        }

        private void OnNewUnitSelected()
        {
            OnPropertyChanged(Properties.Position);
        }

        private void OnMovement()
        {
            OnPropertyChanged(Properties.Position);
        }

        private static class Properties
        {
            public const string Position = "Position";
        }
    }
}
