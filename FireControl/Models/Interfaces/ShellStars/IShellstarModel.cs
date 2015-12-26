using System.Collections.Generic;
using Kinetics.Core.Data;
using Kinetics.Core.Data.FiringSolution;
using Kinetics.Core.Data.HexGrid;

namespace FireControl.Models.Interfaces.ShellStars
{
    internal delegate void CounterfireListChanged(ListAction action, ShellstarInfo affectedShellstar);

    /// <summary>
    /// This is an interface for ShellstarInfo container that allows counterfire on shellstars and placing the shellstar on the map.
    /// Doesn't allow for counterfiring on counterfired salvo.
    /// </summary>
    internal interface IShellstarModel
    {
        /// <summary>
        /// Gets the original shellstar info.
        /// </summary>
        ShellstarInfo Shellstar { get; }

        /// <summary>
        /// Gets time of launch based on impulse track.
        /// </summary>
        TurnData TimeOfLaunch { get; }

        /// <summary>
        /// Gets or sets a string tag to help distinguish between shellstars.
        /// </summary>
        string Tag { get; set; }

        /// <summary>
        /// Gets evasion directions information and relevant data.
        /// </summary>
        IEvasionInfoModel EvasionInfo { get; }

        /// <summary>
        /// Gets shellstar postion on the map on the specified turn and segment.
        /// </summary>
        HexGridCoordinate GetMapPosition(HexGridCoordinate targetPosition, TurnData turnData);

        /// <summary>
        /// Gets the list of salvos of kinetics counterfired against this shellstar.
        /// </summary>
        IReadOnlyCollection<ShellstarInfo> Counterfire { get; }

        /// <summary>
        /// Attaches a child shellstar to this one.
        /// </summary>
        /// <param name="shellstar">Shellstar to attach.</param>
        void AttachCounterfireShellstar(ShellstarInfo shellstar);

        /// <summary>
        /// Detaches an existing counterfire shellstar from this one.
        /// </summary>
        /// <param name="shellstar">Shellstar to detach.</param>
        void DetachCounterfireShellstar(ShellstarInfo shellstar);

        /// <summary>
        /// LNotifies when the list of counterfired kinetics changes.
        /// </summary>
        event CounterfireListChanged CounterfireUpdated;
    }
}