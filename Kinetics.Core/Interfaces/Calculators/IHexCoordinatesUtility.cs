using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;

namespace Kinetics.Core.Interfaces.Calculators
{
    /// <summary>
    /// Provids utility methods that deal with coordinates on hex map for various calculators.
    /// </summary>
    internal interface IHexCoordinatesUtility
    {
        /// <summary>
        /// Shifts coordinate by adding a new component to it.
        /// </summary>
        /// <param name="coordinate">Coordinate being shifted.</param>
        /// <param name="component">Compnoent representing the shift.</param>
        void ShiftCoordinate(HexGridCoordinate coordinate, HexVectorComponent component);

        /// <summary>
        /// Shifts coordinate by the provided vector.
        /// </summary>
        /// <param name="coordinate">Coordinate being shifted.</param>
        /// <param name="vector">Shift vector.</param>
        void ShiftCoordinate(HexGridCoordinate coordinate, HexVector vector);

        /// <summary>
        /// Removes BE component from a hex grid coordinate (which is completely dependent on the DA and CF axis).
        /// </summary>
        /// <param name="position">Hex grid coordinate.</param>
        void EliminateBeComponent(HexGridCoordinate position);
    }
}
