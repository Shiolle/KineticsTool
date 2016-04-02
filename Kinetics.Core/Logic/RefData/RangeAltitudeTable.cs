using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexVectors;
using Kinetics.Core.Interfaces.RefData;
using System;

namespace Kinetics.Core.Logic.RefData
{
    internal class RangeAltitudeTable : IRangeAltitudeTable
    {
        private const double RoundingThreshold = 0.122;

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

        public double RingToLatitude(AvidRing ring)
        {
            return Math.PI * ((byte)ring - 1) / 6d;
        }

        public Tuple<uint, uint> ProjectDirection(AvidRing ring, uint distance)
        {
            if (ring == AvidRing.Ember)
            {
                return new Tuple<uint, uint>(distance, 0);
            }

            if (ring == AvidRing.Magenta)
            {
                return new Tuple<uint, uint>(0, distance);
            }

            // For green and blue rings we have to calculate a centerling: the bisectrix of the sectors these rings represent.
            // It must also satisfy the following condition: for each range value there must be the only pair of horizontal and
            // vertical components that correspond to this range, and no two ranges can have the same coordinates.

            // Preserving the pattern for this centerline that Squadron Strike Vertical Plotting Grid establishes proved tricky
            // because of custom rounding, which, in case of larger component produces changes in its value at irregular intervals.

            // The shorter component is easier. It's 0 at range 0, 1 at range 1 and then increases at each odd range value.
            var shortComponent = (uint)Math.Ceiling(distance / 2d);

            // To reproduce exact pattern of larger component change, a rounding threshold had to be determined imperically. The
            // usual threshold used in mathematics of 0.5 starts producing errors almost immediately. The selected value creates
            // the exact pattern we want up to 50 range. It's impossible to check it further because that's where the RALT ends.
            double longComponentRaw = distance * Math.Cos(Math.PI / 6d);
            uint longComponent = (uint)Math.Floor(longComponentRaw);
            double longComponentRem = longComponentRaw - longComponent;
            if (longComponentRem >= RoundingThreshold || distance % 22 == 0)
            {
                longComponent++;
            }

            if (ring == AvidRing.Blue)
            {
                return new Tuple<uint, uint>(longComponent, shortComponent);
            }
            // Green ring has reverse numbers for horiznotal and vertical components compared to blue ring.
            return new Tuple<uint, uint>(shortComponent, longComponent);
        }
    }
}
