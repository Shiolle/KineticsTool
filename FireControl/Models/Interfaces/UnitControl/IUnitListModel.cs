using System.Collections.Generic;

namespace FireControl.Models.Interfaces.UnitControl
{
    internal delegate void UnitListChanged(ListAction action, IUnitModel affectedUnit);

    /// <summary>
    /// Provides methods to handle a list of units.
    /// </summary>
    internal interface IUnitListModel
    {
        /// <summary>
        /// Gets the list of units.
        /// </summary>
        List<IUnitModel> Models { get; }

        /// <summary>
        /// Notifies when model list has changed.
        /// </summary>
        event UnitListChanged UnitsChanged;

        /// <summary>
        /// Adds a unit to the list.
        /// </summary>
        /// <param name="unitModel">New unit; should not be null.</param>
        void AddUnit(IUnitModel unitModel);

        /// <summary>
        /// Removes unit from a list.
        /// </summary>
        /// <param name="unitModel">Unit to remove. It should have been in the list.</param>
        void RemoveUnit(IUnitModel unitModel);

        /// <summary>
        /// Saves the contents of unit list to specified destination.
        /// </summary>
        /// <param name="filePath">The path to save file.</param>
        void Save(string filePath);

        /// <summary>
        /// Loads the list of units from specified file.
        /// </summary>
        /// <param name="filePath">Full part to the save file.</param>
        void Load(string filePath);
    }
}