using Kinetics.Core.Data.RefData;

namespace Kinetics.Core.Data.FiringSolution
{
    public class FiringSolution
    {
        public AimAdjustment AimAdjustment { get; set; }

        public float RoC { get; set; }

        public int ShotGeometryRow { get; set; }

        public int ShotGeometryColumn { get; set; }

        public int CrossingVector { get; set; }

        public int MuzzleVelocity { get; set; }

        public float CrossingVectorAdjustment { get; set; }

        public float MuzzleVelocityAdjustment { get; set; }

        public int ModifiedCrossingVector { get; set; }

        public int ModifiedMuzzleVelocity { get; set; }

        public int RoCTurn { get; set; }
    }
}
