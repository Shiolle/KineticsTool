using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexVectors;

namespace Kinetics.Core.Interfaces.RefData
{
    public interface IRangeAltitudeTable
    {
        int GetDistance(HexVector vector);

        AvidRing GetRing(HexVector vector);
    }
}
