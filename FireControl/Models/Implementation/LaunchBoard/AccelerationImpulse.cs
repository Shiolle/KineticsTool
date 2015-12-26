using FireControl.Models.Interfaces.LaunchBoard;
using Kinetics.Core.Data.FiringSolution;

namespace FireControl.Models.Implementation.LaunchBoard
{
    internal class AccelerationImpulse : IAccelerationImpulse
    {
        public AccelerationImpulse(MissileAccelerationImpulse accelerationImpulse, float roc)
        {
            Range = accelerationImpulse.Range;
            PositionAdjustment = accelerationImpulse.PositionAdjustment;
            RoC = roc;
        }

        public float Range { get; private set; }

        public float PositionAdjustment { get; private set; }

        public float RoC { get; private set; }
    }
}
