using System;
using Kinetics.Core.Interfaces.Utility;

namespace Kinetics.Core.Logic.Utility
{
    internal class VectorLibrary : IVectorLibrary
    {
        public double GetAngleBetweenVectors(Vector3 a, Vector3 b)
        {
            var cosAlpha = a.Dot(b) / (a.Magnitude * b.Magnitude);

            if (1 - Math.Abs(cosAlpha) < Consts.DoubleEqualityThreshold)
            {
                return cosAlpha < 0 ? Math.PI : 0;
            }

            return Math.Acos(cosAlpha);
        }

        public Vector3 RotateVectorAroundAxis(Vector3 rotatingVector, Vector3 rotationVector, double angle)
        {
            if (Math.Abs(angle) < Consts.DoubleEqualityThreshold)
            {
                return new Vector3(rotatingVector.X, rotatingVector.Y, rotatingVector.Z);
            }

            var rotationQ = new Qauternion(rotationVector, angle);

            return RotateVector(rotatingVector, rotationQ);
        }

        public Vector3[] GetLocalAxis(Vector3 vector, double adjustmentAngle, double referenceAngle)
        {
            double horizontalProjectionLength = Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
            bool isVertical = Math.Abs(horizontalProjectionLength) < Consts.DoubleEqualityThreshold;
            var result = new Vector3[3];
            var rotationVector = Vector3.Up;

            if (isVertical)
            {
                // In this case up vector lies on X axis.
                // We rotate X unit vector around vertical axis to get vertical result 
                double localXAxisAngle = referenceAngle + Math.PI / 2;
                var rotation1 = new Qauternion(rotationVector, localXAxisAngle);
                result[1] = RotateVector(Vector3.Right, rotation1);
                result[0] = vector.Clone(); //No need for adjustment in this case.
            }
            else
            {
                //Construct horizontal projection vector.
                var horizontalProjection = new Vector3(vector.X, vector.Y, 0);
                horizontalProjection.Normalize();

                // Now we have a projection on XY plane of the initial vector. We need to first rotate it 90 degrees around Z axis to find the right vector.
                var rotation1 = new Qauternion(rotationVector, Math.PI / 2);
                result[1] = RotateVector(horizontalProjection, rotation1);

                // If adjustment is required we now rotate original vector by the required angle.
                // We need to rotate it closer to the horizontal plane.
                result[0] = AdjustVectorAsNeeded(vector, result[1], adjustmentAngle);
                double adjustedAngle = GetAngleBetweenVectors(result[0], horizontalProjection) * 180 / Math.PI;
            }

            // And then we can rotate that vector by 90 degrees around the right vector to get our Up vector.
            var rotation2 = new Qauternion(result[0], Math.PI / 2);
            result[2] = RotateVector(result[1], rotation2);

            if (Math.Abs(90 - GetAngleBetweenVectors(result[0], result[1]) * 180 / Math.PI) > Consts.DoubleEqualityThreshold ||
                Math.Abs(90 - GetAngleBetweenVectors(result[1], result[2]) * 180 / Math.PI) > Consts.DoubleEqualityThreshold ||
                Math.Abs(90 - GetAngleBetweenVectors(result[0], result[2]) * 180 / Math.PI) > Consts.DoubleEqualityThreshold)
            {
                throw new ArithmeticException("After trying to find local coordinate axes they were not 90 degrees away from the each other.");
            }
            return result;
        }

        private Vector3 RotateVector(Vector3 vector, Qauternion rotation)
        {
            var rotatedQ = new Qauternion(0, vector.X, vector.Y, vector.Z);

            Qauternion result = rotation * rotatedQ;
            result = result * rotation.GetConjugate();

            return result.Axis;
        }

        private Vector3 AdjustVectorAsNeeded(Vector3 initialVector, Vector3 rotationVector, double adjustmentAngle)
        {
            if (Math.Abs(adjustmentAngle) < Consts.DoubleEqualityThreshold)
            {
                return initialVector.Clone();
            }

            // We need to always move towards horizontal projection.
            var finalAngle = Math.Sign(initialVector.Z) * adjustmentAngle;
            return RotateVectorAroundAxis(initialVector, rotationVector, finalAngle);
        }
    }
}
