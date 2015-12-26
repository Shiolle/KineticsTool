using Kinetics.Core.Data;
using Kinetics.Core.Data.FiringSolution;
using Kinetics.Core.Interfaces.Calculators;
using Kinetics.Core.Interfaces.RefData;
using System;
using Kinetics.Core.Data.RefData;

namespace Kinetics.Core.Logic.Calculators
{
    internal class ShellstarBuilder : IShellstarBuilder
    {
        private readonly IProjectileDamageTable _projectileDamageTable;

        public ShellstarBuilder(IProjectileDamageTable projectileDamageTable)
        {
            _projectileDamageTable = projectileDamageTable;
        }

        public ShellstarInfo BuildShellstarInfo(int targetDistance, TurnData startingImpulse, FiringSolution firingSolution, MissileAccelerationData missileAcceleration)
        {
            if (firingSolution == null)
            {
                throw new ArgumentNullException("firingSolution", "Firing solution is needed to calculate shellstar data.");
            }

            int dmg50,
                dmg100,
                dmg200;

            _projectileDamageTable.GetDamages(firingSolution.RoC, out dmg50, out dmg100, out dmg200);

            var result = new ShellstarInfo
            {
                RoC = firingSolution.RoC,
                Dmg50 = dmg50,
                Dmg100 = dmg100,
                Dmg200 = dmg200,
            };

            float inertialFlightDistance = (missileAcceleration != null) ?
                // Burnout range rounded down.
                (float)Math.Floor(targetDistance - missileAcceleration.BurnDistance) :
                targetDistance;

            BuildInertialSegments(result, inertialFlightDistance, firingSolution, missileAcceleration != null);

            if (missileAcceleration != null)
            {
                BuildAccelerationSegments(result, targetDistance, missileAcceleration);
            }

            result.ImpulseTrack.Reverse();
            AssignImpulseInformation(result, startingImpulse);

            return result;
        }

        private void BuildAccelerationSegments(ShellstarInfo shellstarInfo, int targetDistance, MissileAccelerationData accelData)
        {
            for (int i = 0; i < accelData.BurnDuration; i++)
            {
                int range = i < accelData.BurnDuration - 1 ?
                    (int)(Math.Floor(accelData.ImpulseData[i + 1].Range)) :
                    targetDistance;

                var impulseData = new ImpulseTrackElement
                {
                    Range = range,
                    IsBurning = true
                };
                shellstarInfo.ImpulseTrack.Add(impulseData);
            }
        }

        private void BuildInertialSegments(ShellstarInfo shellstarInfo, float driftDistance, FiringSolution firingSolution, bool isMissile)
        {
            float currentDistance = 0;
            bool roundUp = false;

            // CG shells can be fired at range 0. For this case we add a single segment.
            if ((int)driftDistance == 0 && firingSolution.AimAdjustment != AimAdjustment.NoShot && !isMissile)
            {
                shellstarInfo.ImpulseTrack.Add(new ImpulseTrackElement
                {
                    Range = 0,
                    IsBurning = false
                });
                return;
            }

            // Ex 1: Drift distance = 20; RoC = 4; currentDistance = 20 is satisfies condition, 24 doesn't.
            // Ex 2: Drift distance = 17; RoC = 4; currentDistance = 20 is satisfies condition, 24 doesn't.
            while (driftDistance - currentDistance > - firingSolution.RoC)
            {
                var impulseData = new ImpulseTrackElement
                {
                    Range = (int)Math.Min(currentDistance, driftDistance),
                    IsBurning = false
                };
                shellstarInfo.ImpulseTrack.Add(impulseData);
                currentDistance += roundUp ?
                    (float)Math.Ceiling(firingSolution.RoC) :
                    (float)Math.Floor(firingSolution.RoC);

                roundUp = !roundUp;
            }
        }

        private void AssignImpulseInformation(ShellstarInfo shellstarInfo, TurnData startingImpulse)
        {
            // Need to advance it, so we make a copy.
            var currentImpulse = startingImpulse.DeepCopy();

            for (int i = 0; i < shellstarInfo.ImpulseTrack.Count; i++)
            {
                shellstarInfo.ImpulseTrack[i].Impulse = currentImpulse;
                currentImpulse = currentImpulse.GetNextImpulse();
            }
        }
    }
}
