using Kinetics.Core.Data.RefData;
using Kinetics.Core.Interfaces.RefData;
using System;

namespace Kinetics.Core.Logic.RefData
{
    internal class MissilePositionAdjustmentTable : IMissilePositionAdjustmentTable
    {
        private const int DefaultAcceleration = 8;

        private readonly float[,] _mpatData =
        {
            { 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f },
            { 0,    1.5f, 2f,   3f,   4f,   5f   },
            { 0,    1f,   1.5f, 2f,   2.5f, 3f   }
        };

        public float GetSegmentMissileAdjustment(int burnSegment, int acceleration, AimAdjustment aimAdjustment)
        {
            ValidateParameters(burnSegment, acceleration, aimAdjustment);

            int mpatAccelerationAdjustment = GetMpatAccelerationAdjustment(acceleration);

            return _mpatData[(byte)aimAdjustment, burnSegment - 1 + mpatAccelerationAdjustment];
        }

        public float GetTotalMissileAdjustment(int burnDuration, int acceleration, AimAdjustment aimAdjustment, out int tableColumn)
        {
            ValidateParameters(burnDuration, acceleration, aimAdjustment);
            float totalPositionAdjustment = 0;

            int mpatAccelerationAdjustment = GetMpatAccelerationAdjustment(acceleration);

            for (int i = 0; i < burnDuration; i++)
            {
                totalPositionAdjustment += _mpatData[(byte)aimAdjustment, i + mpatAccelerationAdjustment];
            }

            tableColumn = 7 - burnDuration - mpatAccelerationAdjustment;

            return totalPositionAdjustment;
        }

        private int GetMpatAccelerationAdjustment(int acceleration)
        {
            // Sprint missiles use total and per segment position adjustment from red colum of secon or third column.
            // Ex 1: Burn 1 missile,    Aim: Bearing Window. Mpat Blue = 0.5, Mpat red on impulse 1 = 0.5; MpatAccelerationAdjustment = 0
            // Ex 1: Sprint 16 missile, Aim: Bearing Window. Mpat Blue = 0.5, Mpat red on impulse 1 = 1.5; MpatAccelerationAdjustment = 1
            // Ex 1: Sprint 24 missile, Aim: Bearing Window. Mpat Blue = 0.5, Mpat red on impulse 1 = 2.5; MpatAccelerationAdjustment = 2
            return acceleration / DefaultAcceleration - 1;
        }

        private void ValidateParameters(int burnSegment, int acceleration, AimAdjustment aimAdjustment)
        {
            if (aimAdjustment == AimAdjustment.NoShot)
            {
                throw new ArgumentException("There should be a valid firing solution to reference MPAT.", "aimAdjustment");
            }

            if (burnSegment < 1 || burnSegment > 6)
            {
                throw new ArgumentException("Only burn durations 1 - 6 are supported by MPAT.", "burnSegment");
            }

            if (acceleration == 0 || acceleration % 8 != 0)
            {
                throw new ArgumentException("Acceleration should a multiple of 8, excluding zero.", "acceleration");
            }
        }
    }
}
