using System;
using Kinetics.Core.Data.FiringSolution;
using Kinetics.Core.Data.RefData;
using Kinetics.Core.Interfaces.Calculators;
using Kinetics.Core.Interfaces.RefData;

namespace Kinetics.Core.Logic.Calculators
{
    internal class FiringSolutionCalculator : IFiringSolutionCalculator
    {
        private readonly IShotGeometryTable _shotGeometryTable;
        private readonly IMissilePositionAdjustmentTable _missilePositionAdjustmentTable;

        public FiringSolutionCalculator(IShotGeometryTable shotGeometryTable, IMissilePositionAdjustmentTable missilePositionAdjustmentTable)
        {
            _shotGeometryTable = shotGeometryTable;
            _missilePositionAdjustmentTable = missilePositionAdjustmentTable;
        }

        public FiringSolution CalculateSolution(int courseOffset, int crossingVector, int mvMultiplier, int acceleration)
        {
            var firingSolution = new FiringSolution
            {
                CrossingVector = crossingVector,
                MuzzleVelocity = mvMultiplier * acceleration
            };

            var shotGeometryData = _shotGeometryTable.GetShotGeometry(crossingVector, firingSolution.MuzzleVelocity, courseOffset);

            firingSolution.AimAdjustment = shotGeometryData.AimAdjustment;
            firingSolution.ShotGeometryColumn = shotGeometryData.TableColumn;
            firingSolution.ShotGeometryRow = shotGeometryData.TableRow;
            firingSolution.CrossingVectorAdjustment = shotGeometryData.CvAdjustment;
            firingSolution.MuzzleVelocityAdjustment = shotGeometryData.MvAdjustment;
            firingSolution.ModifiedCrossingVector = (int)Math.Ceiling(firingSolution.CrossingVector * shotGeometryData.CvAdjustment);
            firingSolution.ModifiedMuzzleVelocity = (int)Math.Ceiling(firingSolution.MuzzleVelocity * shotGeometryData.MvAdjustment);
            firingSolution.RoCTurn = firingSolution.ModifiedCrossingVector + firingSolution.ModifiedMuzzleVelocity;
            firingSolution.RoC = CalculateRoc(firingSolution.RoCTurn);

            return firingSolution;
        }

        public MissileAccelerationData CalculateMissileAcceleration(int targetRange, int burnDuration, int acceleration, AimAdjustment aimAdjustment, float roc)
        {
            int tableColumn;
            float totalPositionAdjustment = _missilePositionAdjustmentTable.GetTotalMissileAdjustment(burnDuration, acceleration, aimAdjustment, out tableColumn);

            var result = new MissileAccelerationData
            {
                TotalAcceleration = roc * burnDuration,
                TotalPositionAdjustment = totalPositionAdjustment,
                TableColumn = tableColumn,
                TargetRange = targetRange,
                BurnDuration = burnDuration,
                ValidLaunch = true
            };

            result.BurnDistance = result.TotalAcceleration - result.TotalPositionAdjustment;
            float nextRange = targetRange - result.BurnDistance;

            if (nextRange <= 0)
            {
                result.ValidLaunch = false;
                return result;
            }

            // Filling acceleration data per impulse.
            for (int i = 0; i < burnDuration; i++)
            {
                var currentImpulse = new MissileAccelerationImpulse
                {
                    PositionAdjustment = _missilePositionAdjustmentTable.GetSegmentMissileAdjustment(i + 1, acceleration, aimAdjustment),
                    Range = nextRange
                };

                result.ImpulseData.Add(currentImpulse);

                nextRange = nextRange + roc - currentImpulse.PositionAdjustment;
            }

            if (Math.Abs(nextRange - targetRange) > 0.3f)
            {
                throw new ArithmeticException(string.Format("After computing missile acceleration info, couldn't reach target range at launch ({0}). Final range = {1}",
                                              targetRange, nextRange));
            }

            return result;
        }

        private float CalculateRoc(int rocTurn)
        {
            var result = (float)Math.Floor(Math.Abs((float)rocTurn) / 8f);

            int remainder = Math.Abs(rocTurn) % 8;

            if (remainder >= 6)
            {
                result++;
            }
            else if (remainder >= 3)
            {
                result += 0.5f;
            }

            return result * Math.Sign(rocTurn);
        }
    }
}
