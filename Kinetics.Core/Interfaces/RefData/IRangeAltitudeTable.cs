using System;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexVectors;

namespace Kinetics.Core.Interfaces.RefData
{
    public interface IRangeAltitudeTable
    {
        /// <summary>
        /// Gets the length of the hex vector.
        /// </summary>
        /// <param name="vector">Hex vector to calculate the distance.</param>
        /// <returns>The lenth of the provided hex vector.</returns>
        int GetDistance(HexVector vector);

        /// <summary>
        /// Determines which AVID ring is an object seen through if the relative position to the observer is given.
        /// </summary>
        /// <param name="vector">The hex vector of relative position between the observer and the observed.</param>
        /// <returns>AVID ring that corresponds to the given relative position.</returns>
        AvidRing GetRing(HexVector vector);

        /// <summary>
        /// Gets the angle from horizontal (Ember) plane to the ring's bisecting line.
        /// </summary>
        /// <param name="ring">AVID ring.</param>
        /// <returns>Angle from hotizontal plane to ring's bisecting line (centerline).</returns>
        double RingToLatitude(AvidRing ring);

        /// <summary>
        /// Determines horizontal and vertical components by ring and total distance. Needed to project incoming
        /// shellstar back to the map.
        /// </summary>
        /// <param name="ring">AVID ring.</param>
        /// <param name="distance">Total distance.</param>
        /// <returns>Returns a tuple: first component is horizontal and second is vertical.</returns>
        Tuple<uint, uint> ProjectDirection(AvidRing ring, uint distance);
    }
}
