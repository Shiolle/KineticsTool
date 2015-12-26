using Kinetics.Core.Data.FiringSolution;
using Kinetics.Core.Data.RefData;

namespace Kinetics.Core.Interfaces.Calculators
{
    public interface IFiringSolutionCalculator
    {
        FiringSolution CalculateSolution(int courseOffset, int crossingVector, int mvMultiplier, int acceleration);

        MissileAccelerationData CalculateMissileAcceleration(int targetRange, int burnDuration, int acceleration, AimAdjustment aimAdjustment, float roc);
    }
}
