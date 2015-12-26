using Kinetics.Core.Data.RefData;
using Kinetics.Core.Interfaces.RefData;
using Sgs = Kinetics.Core.Data.RefData.ShotGeometryCondition;

namespace Kinetics.Core.Logic.RefData
{
    internal class ShotGeometryTable : IShotGeometryTable
    {
        // Column number is shot offset. Row number is an index for MV adjustment array.
        // Aim: No Shot row is not needed, because it's basically 'else' condition.
        private readonly ShotGeometryCondition[,] _shotGeometryData =
        {
            { Sgs.L10_10,  Sgs.L10_10,  Sgs.L05_10,  Sgs.L05_10,  Sgs.L05_10,  Sgs.L10_10,  Sgs.Always },
            { Sgs.None,    Sgs.None,    Sgs.L10_10,  Sgs.L10_10,  Sgs.L10_10,  Sgs.L15_10,  Sgs.None   },
            { Sgs.None,    Sgs.None,    Sgs.None,    Sgs.None,    Sgs.L15_10,  Sgs.L20_10,  Sgs.None   }
        };

        private readonly float[] _cvAdjustmentData =
            { -1.0f,       -0.9f,       -0.5f,       0,           0.5f,        0.9f,        1.0f };

        private readonly float[] _mvAdjustmentData = { 1.0f, 0.9f, 0.5f, 0 };

        private readonly AimAdjustment[] _aimAdjustmentData =
            { AimAdjustment.BearingWindow, AimAdjustment.Shift1Window, AimAdjustment.Shift2Window, AimAdjustment.NoShot };

        public ShotGeometryTableResult GetShotGeometry(int crossingVector, int muzzleVelocity, int courseOffset)
        {
            var result = new ShotGeometryTableResult
            {
                TableColumn = 6 - courseOffset, // Column count is zero-base, but actual columnt order in the table is reversed.
                CvAdjustment = _cvAdjustmentData[courseOffset]
            };

            int selectedRow = 0;
            bool isConditionMet = false;
            while (selectedRow < 3 && !isConditionMet)
            {
                isConditionMet = IsConditionSatisfied(selectedRow, crossingVector, muzzleVelocity, courseOffset);
                if (!isConditionMet)
                {
                    selectedRow++;
                }
            }

            result.TableRow = selectedRow;
            result.AimAdjustment = _aimAdjustmentData[selectedRow];
            result.MvAdjustment = _mvAdjustmentData[selectedRow];

            return result;
        }

        private bool IsConditionSatisfied(int row, int crossingVector, int muzzleVelocity, int courseOffset)
        {
            ShotGeometryCondition condition = _shotGeometryData[row, courseOffset];
            double relation = ((double)crossingVector) / ((double)muzzleVelocity);
            switch (condition)
            {
                case Sgs.Always:
                    return true;
                case Sgs.L05_10:
                    return relation < 0.5f;
                case Sgs.L10_10:
                    return relation < 1;
                case Sgs.L15_10:
                    return relation < 1.5f;
                case Sgs.L20_10:
                    return relation < 2;
                default:
                    return false;
            }
        }
    }
}
