using FireControl.Models.Interfaces.LaunchBoard;

namespace FireControl.Models.Implementation.LaunchBoard
{
    internal class WeaponSelectionModel : IWeaponSelectionModel
    {
        public WeaponSelectionModel()
        {
            MuzzleVelocityMultiplyer = 0;
            Acceleration = 8;
            IsMissile = false;
        }

        public int MuzzleVelocityMultiplyer { get; set; }
        public bool IsMissile { get; set; }
        public int Acceleration { get; set; }

        public bool IsWeaponSelected
        {
            get { return MuzzleVelocityMultiplyer > 0; }
        }
    }
}
