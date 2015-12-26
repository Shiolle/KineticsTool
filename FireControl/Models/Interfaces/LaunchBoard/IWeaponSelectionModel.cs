namespace FireControl.Models.Interfaces.LaunchBoard
{
    /// <summary>
    /// Provides necessary information for weapons selection and stores selection result.
    /// </summary>
    internal interface IWeaponSelectionModel
    {
        /// <summary>
        /// Gets or sets muzzle velocity multiplyer equal to Muzzle Velocity /8 for railguns or burn duration for missiles.
        /// </summary>
        int MuzzleVelocityMultiplyer { get; set; }

        /// <summary>
        /// Gets or sets whether the weapon can accelerate. True if it can; false otherwise.
        /// </summary>
        bool IsMissile { get; set; }

        /// <summary>
        /// Gets or sets acceleration of the weapon, or how much each MuzzleVelocityMultiplier point adds to the MV.
        /// For coilgun and normal missiles it is 8; needed for sprint missiles.
        /// </summary>
        int Acceleration { get; set; }

        /// <summary>
        /// Gets true if there is a valid weapon selection; otherwise false.
        /// </summary>
        bool IsWeaponSelected { get; }
    }
}
