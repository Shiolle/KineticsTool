using Kinetics.Core.Logic.Utility;

namespace Kinetics.Core.Interfaces.Utility
{
    /// <summary>
    /// Provides methods for manipulating vectors and managing spherical geometry.
    /// </summary>
    internal interface IVectorLibrary
    {
        /// <summary>
        /// Gets angle in radians between two vectors.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>Angle between a and b in radians.</returns>
        double GetAngleBetweenVectors(Vector3 a, Vector3 b);

        /// <summary>
        /// Rotates vector around provided axis, also defined by vector, by a given angle.
        /// </summary>
        /// <param name="rotatingVector">Vector that will be rotated.</param>
        /// <param name="rotationVector">Vector that serves as rotation axis.</param>
        /// <param name="angle"></param>
        /// <returns>The result of rotation.</returns>
        Vector3 RotateVectorAroundAxis(Vector3 rotatingVector, Vector3 rotationVector, double angle);

        /// <summary>
        /// Gets a set of three axis if the provided vector points along X axis and global Z axis lies in local XZ plane.
        /// </summary>
        /// <param name="vectorX">X axis of the local coordinate system.</param>
        /// <param name="adjustmentAngle">An angle in radians by which the inital vector will be lowered towards XY plane. This is done to be able to fine tune
        /// AVID projection, since AVID windows are 30 degrees segments.</param>
        /// <param name="referenceAngle">An angle from X axis to reference direction, needed in case vectorX is vertical.</param>
        /// <returns>An array of 3 vectors. Element 0 is always the same as vectorX. Element 1 is Y axis which points to the right when looking along Z axis;
        /// in this version it always lies on global XY plane. Element 2 is local Z axis; global Z axis lies on the local XZ plane.</returns>
        Vector3[] GetLocalAxis(Vector3 vectorX, double adjustmentAngle, double referenceAngle);
    }
}
