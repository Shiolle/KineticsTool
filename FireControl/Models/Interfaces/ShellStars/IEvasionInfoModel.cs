using System;
using Kinetics.Core.Data.Avid;

namespace FireControl.Models.Interfaces.ShellStars
{
    /// <summary>
    /// Provides access to all information on evasion directions on a shellstar.
    /// </summary>
    internal interface IEvasionInfoModel
    {
        /// <summary>
        /// Returns true if impact window and evasion directions have values.
        /// </summary>
        bool DirectionsDefined { get; }

        /// <summary>
        /// Gets impact window.
        /// </summary>
        AvidWindow ImpactWindow { get; }

        /// <summary>
        /// Gets Úp evasion direction.
        /// </summary>
        AvidWindow EvasionUp { get; }

        /// <summary>
        /// Gets Down evasion direction.
        /// </summary>
        AvidWindow EvasionDown { get; }

        /// <summary>
        /// Gets Left evasion direction.
        /// </summary>
        AvidWindow EvasionLeft { get; }

        /// <summary>
        /// Gets Right evasion direction.
        /// </summary>
        AvidWindow EvasionRight { get; }

        /// <summary>
        /// Updates impact window based on the launch window and reference direction.
        /// </summary>
        /// <param name="launchWindow">Updates evasion directions based on launch window.</param>
        /// <param name="referenceDirection">Reference direction is needed in case whenre launch window is directly above or beyond the target.
        /// In this case reference direction becomes Up evasion direction.</param>
        void UpdateDirections(AvidWindow launchWindow, AvidDirection referenceDirection);

        /// <summary>
        /// Invoked after evasion directions change.
        /// </summary>
        event Action EvasionDirectionsChanged;
    }
}
