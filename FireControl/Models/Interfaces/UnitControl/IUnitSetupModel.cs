namespace FireControl.Models.Interfaces.UnitControl
{
    internal delegate void SelectedUnitChangedEventHandler(SelectionSlot slot, IUnitModel newUnit);

    internal interface IUnitSetupModel
    {
        event SelectedUnitChangedEventHandler SelectionChanged;

        /// <summary>
        /// Gets unit in the first selection slot.
        /// </summary>
        IUnitModel FirstUnit { get; }

        /// <summary>
        /// Gets unit in the second selection slot.
        /// </summary>
        IUnitModel SecondUnit { get; }

        /// <summary>
        /// Returns true if both selection slotts are filled; otherwise false.
        /// </summary>
        bool BothUnitsSelected { get; }

        /// <summary>
        /// Update selection.
        /// </summary>
        /// <param name="newUnit">Unit that will be added to selection or replace existing unit.</param>
        /// <param name="slot">Selection slot to update</param>
        void UpdateSelectedUnit(IUnitModel newUnit, SelectionSlot slot);

        /// <summary>
        /// Selects unit that is the pair for the one in the specified slot.
        /// </summary>
        /// <param name="slot">Selection slot.</param>
        /// <returns>Returns a pair unit for specifeid slot; if no pair exits returns null</returns>
        IUnitModel GetPair(SelectionSlot slot);
    }
}
