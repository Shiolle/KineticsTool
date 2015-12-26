using System;
using System.Linq;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Interfaces.Calculators;
using Kinetics.Core.Interfaces.RefData;
using Kinetics.Core.Interfaces.Utility;
using Kinetics.Core.Logic.Utility;

namespace Kinetics.Core.Logic.Calculators
{
    internal class AvidCalculator : IAvidCalculator
    {
        private readonly IRangeAltitudeTable _rangeAltitudeTable;
        private readonly IVectorLibrary _vectorLibrary;
        private readonly IAvidPathfinder _avidPathfinder;
        private readonly AvidModel _avidModel;

        public AvidCalculator(IRangeAltitudeTable rangeAltitudeTable, IVectorLibrary vectorLibrary, IAvidPathfinder avidPathfinder, IAvidModelBuilder avidModelBuilder)
        {
            if (rangeAltitudeTable == null)
            {
                throw new ArgumentException("Range-altitude table does not exist.", "rangeAltitudeTable");
            }

            _rangeAltitudeTable = rangeAltitudeTable;
            _vectorLibrary = vectorLibrary;
            _avidModel = avidModelBuilder.BuildModel();
            _avidPathfinder = avidPathfinder;
        }

        public AvidVector ProjectVectorToAvid(HexVector vector)
        {
            var result = new AvidVector
            {
                Magnitude = _rangeAltitudeTable.GetDistance(vector)
            };

            if (result.Magnitude == 0)
            {
                return result;
            }

            result.Ring = _rangeAltitudeTable.GetRing(vector);
            result.AbovePlane = result.Ring == AvidRing.Ember || vector.VerticalComponent.Direction == HexAxis.Up;
            result.Direction = CalculateDirection(vector, result.Ring);

            return result;
        }

        private AvidDirection CalculateDirection(HexVector vector, AvidRing ring)
        {
            if (ring == AvidRing.Magenta)
            {
                return AvidDirection.Undefined;
            }

            if (vector.PlanarProjection == 0)
            {
                throw new ArithmeticException("The ring is not magenta, but planar projection is zero.");
            }

            if (vector.PrimaryComponent.Direction == vector.SecondaryComponent.Direction && vector.SecondaryComponent.Magnitude != 0)
            {
                throw new ArithmeticException("Primary and secondary vectors are both positive but are in the same direction.");
            }

            int primaryMagnitude = vector.PrimaryComponent.Magnitude;
            int secondaryMagnitude = vector.SecondaryComponent.Magnitude;

            if (primaryMagnitude < secondaryMagnitude)
            {
                throw new ArithmeticException("Secondary vector component should not be greater than the primary.");
            }

            if (IsSeeingThroughHexEdge(primaryMagnitude, secondaryMagnitude))
            {
                return (AvidDirection)(((byte)vector.PrimaryComponent.Direction) * 2 - 1);
            }

            if ((vector.PrimaryComponent.Direction == HexAxis.A && vector.SecondaryComponent.Direction == HexAxis.F) ||
                (vector.PrimaryComponent.Direction == HexAxis.F && vector.SecondaryComponent.Direction == HexAxis.A))
            {
                return AvidDirection.FA;
            }

            return (AvidDirection)(((byte)vector.PrimaryComponent.Direction) + ((byte)vector.SecondaryComponent.Direction) - 1);
        }

        public double DirectionToAngle(AvidDirection direction)
        {
            return Math.PI * ((16 - (byte)direction) % 12) / 6d;
        }

        public AvidWindow[] GetRotationCircleFromNormal(AvidWindow rotationAxis)
        {
            bool adjustmentNeeded = rotationAxis.Ring == AvidRing.Blue || rotationAxis.Ring == AvidRing.Green;

            Vector3 axisVector = WindowToVector3(rotationAxis);
            var localAxis = _vectorLibrary.GetLocalAxis(axisVector, adjustmentNeeded ? Consts.RotationAdjustmentAngle : 0d, 0d);
            axisVector = localAxis[0];
            Vector3 upVector = localAxis[2];
            var result = new AvidWindow[12];
            //There are always 12 windows if the rotation circle is continuous.
            for (int i = 0; i < 12; i++)
            {
                //Rotation is always clockwise.
                Vector3 rotatedAxis = _vectorLibrary.RotateVectorAroundAxis(upVector, axisVector, -i * Math.PI / 6d);
                result[i] = Vector3ToAvid(rotatedAxis);
            }
            return result;
        }

        public AvidWindow[] GetPossibleLaunchWindows(int courseOffset, AvidWindow targetWindow, AvidWindow crossingVector)
        {
            if (AvidWindow.IsNullOrZero(targetWindow))
            {
                return new[] { GetOppositeWindow(crossingVector) }; //This may still result in undefined vector if both are zero.
                // This is fine. If both ships aren't moving and sit in the same hex, launch window can't be determined. This situation should
                // not arise in any real scenario, but we still need to handle it.
            }

            if (courseOffset == 0 || targetWindow.Equals(crossingVector) || AvidWindow.IsNullOrZero(crossingVector))
            {
                return new [] { targetWindow };
            }

            var pathingResult = _avidPathfinder.GetShortestPaths(_avidModel, targetWindow, crossingVector, AvidPathingOptions.DiagonalTransitionsLimitWithPolar);

            if (!pathingResult.PathExists)
            {
                return new AvidWindow[] { };
            }

            return pathingResult.AllShortestPaths.Select(pi => pi.PathNodes[courseOffset])
                .Where(lan => lan.Window.MinDistance >= lan.NodeDistance) // This is to exclude windows, that could be reached through diagonal transition
                // but can also reached by two non-diagonal transitions. So, in the final result for course offset = 2, it would look like two windows
                .Select(lan => (AvidWindow)lan.Window).Distinct().ToArray();
        }

        public int CountWindows(AvidWindow start, AvidWindow destination)
        {
            if (start.Equals(destination) || AvidWindow.IsNullOrZero(start) || AvidWindow.IsNullOrZero(destination))
            {
                return 0;
            }

            var pathingResult = _avidPathfinder.GetShortestPaths(_avidModel, start, destination, AvidPathingOptions.DiagonalTransitionsLimitWithPolar);

            if (!pathingResult.PathExists)
            {
                throw new Exception("Failed to find path from start to destination. That must be the problem with the algorithm.");
            }

            if (Math.Abs(pathingResult.MinimalDistance - GetDistanceFromAngle(start, destination) * 2) > 1)
            {
                //throw new Exception("Distance inconsistent.");
            }

            // Since all normal transitions on the avid are two points, we need to halve the result.
            return pathingResult.MinimalDistance / 2;
        }

        public int GetCourseOffset(AvidWindow targetVector, AvidWindow crossingVector)
        {
            if (AvidWindow.IsNullOrZero(targetVector) || AvidWindow.IsNullOrZero(crossingVector))
            {
                return 6;
            }

            return CountWindows(targetVector, crossingVector);
        }

        public int GetDistanceFromAngle(AvidWindow a, AvidWindow b)
        {
            return (int)(6 * _vectorLibrary.GetAngleBetweenVectors(WindowToVector3(a), WindowToVector3(b)) / Math.PI);
        }

        public AvidOrientation GetOrientation(AvidWindow nose, AvidWindow top)
        {
            throw new NotImplementedException();
        }

        public AvidOrientation GetOrientationWithoutRoll(AvidWindow nose, AvidDirection referenceDirection)
        {
            Vector3 vectorX = WindowToVector3(nose);
            var axis = _vectorLibrary.GetLocalAxis(vectorX, 0d, DirectionToAngle(referenceDirection));
            var result = new AvidOrientation
            {
                Nose = Vector3ToAvid(axis[0]),
                Starboard = Vector3ToAvid(axis[1]),
                Top = Vector3ToAvid(axis[2]),
            };

            result.Aft = GetOppositeWindow(result.Nose);
            result.Port = GetOppositeWindow(result.Starboard);
            result.Bottom = GetOppositeWindow(result.Top);
            return result;
        }

        public double RingToLatitude(AvidRing ring)
        {
            return Math.PI * ((byte)ring - 1) / 6d;
        }

        public AvidWindow GetOppositeWindow(AvidWindow window)
        {
            var oppositeDirection = window.Direction == AvidDirection.Undefined ? window.Direction : (AvidDirection)((((byte)window.Direction) + 5) % 12 + 1);
            return new AvidWindow(oppositeDirection, window.Ring, window.Ring == AvidRing.Ember || !window.AbovePlane);
        }

        private bool IsSeeingThroughHexEdge(int distanceA, int distanceB)
        {
            return distanceA == 0 || distanceB == 0 ||
                   (distanceA / distanceB >= 3) || (distanceB / distanceA >= 3);
        }

        private Vector3 WindowToVector3(AvidWindow rotationAxis)
        {
            double angleA = DirectionToAngle(rotationAxis.Direction);
            double angleB = rotationAxis.AbovePlane ? RingToLatitude(rotationAxis.Ring) : 2 * Math.PI - RingToLatitude(rotationAxis.Ring);
            return new Vector3(Math.Cos(angleA) * Math.Cos(angleB), Math.Sin(angleA) * Math.Cos(angleB), Math.Sin(angleB));
        }

        private AvidWindow Vector3ToAvid(Vector3 vector)
        {
            
            var result = new AvidWindow
            {
                Ring = CoordinateToRing(vector.Z)
            };
            result.Direction = result.Ring != AvidRing.Magenta ? GetDirection(vector) : AvidDirection.Undefined;
            result.AbovePlane = vector.Z >= 0 || result.Ring == AvidRing.Ember;
            return result;
        }

        private AvidDirection GetDirection(Vector3 vector)
        {
            var horizontalProjection = Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
            if (horizontalProjection < Consts.DoubleEqualityThreshold)
            {
                return AvidDirection.Undefined;
            }

            double horizontalAngle = Math.Atan2(vector.Y, vector.X);

            if (horizontalAngle < 0)
            {
                horizontalAngle = 2 * Math.PI + horizontalAngle;
            }

            // We will now determine trigonometric zone. This zones, as AVID zones go in 30 degrees increment, but counterclockwise and starting from 0.
            var trigonometricZone = (byte)(6d * (horizontalAngle + Math.PI / 12d) / Math.PI);
            
            // Now convert it to AVID zone where directions go counterclockwise from Pi/2.
            return (AvidDirection)((15 - trigonometricZone) % 12 + 1);
        }

        private AvidRing CoordinateToRing(double latitude)
        {
            double absLatitude = Math.Abs(latitude);
            if (absLatitude <= Math.Sin(Math.PI / 12))
            {
                return AvidRing.Ember;
            }

            if (absLatitude <= Math.Sin(Math.PI / 4))
            {
                return AvidRing.Blue;
            }

            if (absLatitude <= Math.Sin(5 * Math.PI / 12))
            {
                return AvidRing.Green;
            }

            return AvidRing.Magenta;
        }
    }
}
