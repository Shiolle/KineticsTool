using System;
using System.Collections.Generic;
using FireControl.Models.Interfaces.ShellStars;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;

namespace FireControl.Models.Interfaces.UnitControl
{
    internal delegate void ShellstarListChangedDelegate(ListAction action, IShellstarModel affectedShellstar);

    /// <summary>
    /// Lists minimum unit information.
    /// </summary>
    internal interface IUnitModel
    {
        /// <summary>
        /// Gets or sets unit's name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets unit's position.
        /// </summary>
        HexGridCoordinate Position { get; set; }

        /// <summary>
        /// Gets or sets unit's velocity vectors.
        /// </summary>
        RawHexVector Vectors { get; set; }

        /// <summary>
        /// Gets the list of ShellStars fired against this unit.
        /// </summary>
        IReadOnlyCollection<IShellstarModel> AttachedShellstars { get; }

        /// <summary>
        /// Moves the unit in the specified direction by 1.
        /// </summary>
        /// <param name="direction">Direction of the movement.</param>
        void Move(HexAxis direction);

        /// <summary>
        /// Returns the sum of all vector components along a direction.
        /// </summary>
        /// <param name="direction">Hex direction.</param>
        /// <returns>The sum of all component magnitudes along the specified direction.</returns>
        int GetDirectionValue(HexAxis direction);

        /// <summary>
        /// Makes it so there is a single component in the given direction with magnitude equal to the value. If value is zero, all components are removed.
        /// </summary>
        /// <param name="direction">Direction</param>
        /// <param name="newValue">Value. If the value is zero all components in the given direction are removed.</param>
        void AssginDirectionValue(HexAxis direction, uint newValue);

        /// <summary>
        /// Attaches a new shellstar.
        /// </summary>
        /// <param name="shellstar">Shellstar to attach.</param>
        void AttachShellstar(IShellstarModel shellstar);

        /// <summary>
        /// Detaches the specified shellstar from this unti. Throws an axception if the shellstar wasn't attached.
        /// </summary>
        /// <param name="shellstar">Shellstar to detach.</param>
        void DetachShellstar(IShellstarModel shellstar);

        /// <summary>
        /// Fired when unit moves.
        /// </summary>
        event Action Moved;

        /// <summary>
        /// Fired when unit changes velocity.
        /// </summary>
        event Action VelocityChanged;

        /// <summary>
        /// Fires when a unit is assigned an new shellstar or when a shellstar is removed.
        /// </summary>
        event ShellstarListChangedDelegate ShellstarListChanged;
    }
}
