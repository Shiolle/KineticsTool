using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;

namespace Kinetics.Core.Interfaces.Calculators
{
    /// <summary>
    /// Provides methods for going from Hex Map positions to AVID vectors and back.
    /// </summary>
    public interface IAvidProjectionCalculator
    {
        /// <summary>
        /// Allows to determine through which window one object is seeing the other. It doesn't matter whether the vector is hex coordinates or a velocity difference.
        /// </summary>
        /// <param name="vector">Hex vector defining two objects' relative positions.</param>
        /// <returns>Avid window through which an object would see the other object if the specified defines their relative position.</returns>
        AvidVector ProjectVectorToAvid(HexVector vector);

        /// <summary>
        /// Gets all possible coordinates for projecting AVID vector back onto Hex Grid.
        /// </summary>
        /// <param name="vector">Avid vector to project.</param>
        /// <param name="position">Position of the object that has this AVID vector on Hex Grid.</param>
        /// <returns>Returns the list of possible Hex Grid coordinates for the specified vector projection.</returns>
        HexGridCoordinate[] ProvectVectorToMap(AvidVector vector, HexGridCoordinate position);
    }
}
