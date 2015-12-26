using Kinetics.Storage.Configuration.CoilgunTypes;
using Kinetics.Storage.Configuration.MissileTypes;

namespace Kinetics.Storage
{
    /// <summary>
    /// This interface provides access to application configuration for FireControl.
    /// </summary>
    public interface IStaticConfigurationController
    {
        /// <summary>
        /// Gets starting position where units appear by default when created.
        /// </summary>
        string StartingUnitPosition { get; }

        /// <summary>
        /// Gets the list of coilgun types.
        /// </summary>
        CoilgunType[] CoilgunTypes { get; }

        /// <summary>
        /// Get the list of missile types.
        /// </summary>
        MissileType[] MissileTypes { get; }
    }
}
