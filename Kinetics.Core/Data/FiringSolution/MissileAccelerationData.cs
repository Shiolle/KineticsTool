using System.Collections.Generic;

namespace Kinetics.Core.Data.FiringSolution
{
    public class MissileAccelerationData
    {
        public MissileAccelerationData()
        {
            ImpulseData = new List<MissileAccelerationImpulse>();
        }

        public List<MissileAccelerationImpulse> ImpulseData { get; set; }

        public bool ValidLaunch { get; set; }

        public int BurnDuration { get; set; }

        public int TableColumn { get; set; }

        public float TotalAcceleration { get; set; }

        public float TotalPositionAdjustment { get; set; }

        public float TargetRange { get; set; }

        public float BurnDistance { get; set; }
    }
}
