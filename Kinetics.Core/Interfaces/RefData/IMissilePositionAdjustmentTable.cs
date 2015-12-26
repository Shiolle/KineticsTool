using Kinetics.Core.Data.RefData;

namespace Kinetics.Core.Interfaces.RefData
{
    public interface IMissilePositionAdjustmentTable
    {
        float GetSegmentMissileAdjustment(int burnSegment, int acceleration, AimAdjustment aimAdjustment);

        float GetTotalMissileAdjustment(int burnDuration, int acceleration, AimAdjustment aimAdjustment, out int tableColumn);
    }
}
