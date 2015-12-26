using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexVectors;
using Kinetics.Core.Interfaces.RefData;
using System;

namespace Kinetics.Core.Logic.RefData
{
    internal class RangeAltitudeTable : IRangeAltitudeTable
    {
        public int GetDistance(HexVector vector)
        {
            return (int)Math.Floor(Math.Sqrt(Math.Pow(vector.PlanarProjection, 2) + Math.Pow(vector.VerticalComponent.Magnitude, 2)));
        }

        public AvidRing GetRing(HexVector vector)
        {
            int horizontalDistance = vector.PlanarProjection;
            int verticalDistance = vector.VerticalComponent.Magnitude;
            if (horizontalDistance >= 4 * verticalDistance)
            {
                return AvidRing.Ember;
            }

            if (horizontalDistance >= verticalDistance)
            {
                return AvidRing.Blue;
            }

            if (4 * horizontalDistance <= verticalDistance)
            {
                return AvidRing.Magenta;
            }

            return AvidRing.Green;
        }
    }
}
