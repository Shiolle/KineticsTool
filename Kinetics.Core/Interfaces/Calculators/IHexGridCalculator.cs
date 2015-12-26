using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;

namespace Kinetics.Core.Interfaces.Calculators
{
    /// <summary>
    /// Provides methods to manipulate hex grid coordinates.
    /// </summary>
    public interface IHexGridCalculator
    {
        void Move(HexGridCoordinate position, HexAxis direction, uint distance);

        AvidVector GetDistance(HexGridCoordinate from, HexGridCoordinate to);

        AvidVector GetCrossingVector(RawHexVector attackerVelocity, RawHexVector targetVelocity);

        /// <summary>
        /// Implements rule D2.16, calculating target bearing when 
        /// </summary>
        /// <param name="observerVelocity">Velocity of the observer.</param>
        /// <param name="observedVelocity">Velocity of the observed.</param>
        /// <returns>Target bearing.</returns>
        AvidVector GetVectorFromBacktracking(RawHexVector observerVelocity, RawHexVector observedVelocity);
    }
}
