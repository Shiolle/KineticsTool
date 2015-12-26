using System.Collections.Generic;
using System.Linq;
using FireControl.Models.Interfaces;
using FireControl.Models.Interfaces.UnitControl;

namespace FireControl.Models.Implementation.UnitControl
{
    internal class UnitSetupModel : IUnitSetupModel
    {
        private readonly Dictionary<SelectionSlot, IUnitModel> _selectedUnits;

        public UnitSetupModel(IUnitListModel unitListModel)
        {
            _selectedUnits = new Dictionary<SelectionSlot, IUnitModel>();
            unitListModel.UnitsChanged += UnitListModelOnUnitsChanged;
        }

        public event SelectedUnitChangedEventHandler SelectionChanged;

        /// <summary>
        /// Gets unit in the first selection slot.
        /// </summary>
        public IUnitModel FirstUnit
        {
            get { return GetUnit(SelectionSlot.FirstUnit); }
        }

        /// <summary>
        /// Gets unit in the second selection slot.
        /// </summary>
        public IUnitModel SecondUnit
        {
            get { return GetUnit(SelectionSlot.SecondUnit); }
        }

        /// <summary>
        /// Returns true if both selection slotts are filled; otherwise false.
        /// </summary>
        public bool BothUnitsSelected
        {
            get
            {
                return _selectedUnits.Count == 2 && //Both slots are filled.
                       _selectedUnits[SelectionSlot.FirstUnit] != _selectedUnits[SelectionSlot.SecondUnit];
            }
        }

        /// <summary>
        /// Update selection.
        /// </summary>
        /// <param name="newUnit">Unit that will be added to selection or replace existing unit.</param>
        /// <param name="slot">Selection slot to update</param>
        public void UpdateSelectedUnit(IUnitModel newUnit, SelectionSlot slot)
        {
            _selectedUnits[slot] = newUnit;
            if (SelectionChanged != null)
            {
                SelectionChanged(slot, newUnit);
            }
        }

        public IUnitModel GetPair(SelectionSlot slot)
        {
            return slot == SelectionSlot.FirstUnit ? SecondUnit : FirstUnit;
        }

        private IUnitModel GetUnit(SelectionSlot slot)
        {
            IUnitModel unit;
            _selectedUnits.TryGetValue(slot, out unit);

            return unit;
        }

        private void UnitListModelOnUnitsChanged(ListAction action, IUnitModel affectedUnit)
        {
            switch (action)
            {
                case ListAction.Reset:
                    ResetSelection();
                    return;
                case ListAction.Removed:
                    Deselect(affectedUnit);
                    return;
            }
        }

        private void Deselect(IUnitModel affectedUnit)
        {
            var affectedKeys = _selectedUnits.Where(kvp => kvp.Value == affectedUnit).Select(kvp => kvp.Key).ToArray();
            foreach (var key in affectedKeys)
            {
                _selectedUnits.Remove(key);
                SelectionChanged(key, null);
            }
        }

        private void ResetSelection()
        {
            _selectedUnits.Clear();
            SelectionChanged(SelectionSlot.FirstUnit, null);
            SelectionChanged(SelectionSlot.SecondUnit, null);
        }
    }
}
