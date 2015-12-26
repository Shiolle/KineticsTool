using Kinetics.Core.Data.RefData;

namespace Kinetics.Core.Interfaces.RefData
{
    public interface IShotGeometryTable
    {
        ShotGeometryTableResult GetShotGeometry(int crossingVector, int muzzleVelocity, int courseOffset);
    }
}
