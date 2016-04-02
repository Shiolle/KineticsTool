using System;
using System.Collections.Generic;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;
using Kinetics.Core.Interfaces.Calculators;
using Kinetics.Core.Interfaces.RefData;

namespace Kinetics.Core.Logic.Calculators
{
    internal class AvidProjectionCalculator : IAvidProjectionCalculator
    {
        private readonly IRangeAltitudeTable _rangeAltitudeTable;
        private readonly IHexCoordinatesUtility _hexCoordinatesUtility;

        public AvidProjectionCalculator(IRangeAltitudeTable rangeAltitudeTable, IHexCoordinatesUtility hexCoordinatesUtility)
        {
            _rangeAltitudeTable = rangeAltitudeTable;
            _hexCoordinatesUtility = hexCoordinatesUtility;
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

        public HexGridCoordinate[] ProvectVectorToMap(AvidVector vector, HexGridCoordinate position)
        {
            Tuple<uint, uint> projections = _rangeAltitudeTable.ProjectDirection(vector.Ring, (uint)vector.Magnitude);

            HexAxis[] hexAxis = GetAxisFromFacing(vector.Direction);

            if (hexAxis.Length == 0 || hexAxis.Length > 2)
            {
                throw new Exception(string.Format("An AVID direction should be split in one or two hex grid directions, but encountered {0}", hexAxis.Length));
            }

            var result = new List<HexGridCoordinate>();
            var newCoordinate = position.Clone();
            var shiftVector = new HexVector();

            shiftVector.AddComponent(vector.AbovePlane ? HexAxis.Up : HexAxis.Down, (int)projections.Item2);

            if (hexAxis.Length == 1) //One hex direction
            {
                shiftVector.AddComponent(hexAxis[0], (int)projections.Item1);
            }
            // If the AVID vector looks through hex corner, we need to split the horizontal projection evenly among the two adjacent directions.
            // That may mean, if the horizontal projection is an odd number, there can be two potential coordinates.
            else
            {
                int halfProjection = (int)projections.Item1 / 2;
                shiftVector.AddComponent(hexAxis[0], halfProjection);
                shiftVector.AddComponent(hexAxis[1], halfProjection);

                // If the horizontal projection is an odd number, this last hex of movement can go into any of the two directions.
                if (projections.Item1 % 2 == 1)
                {
                    var secondCoordinate = newCoordinate.Clone();
                    _hexCoordinatesUtility.ShiftCoordinate(secondCoordinate, shiftVector);
                    _hexCoordinatesUtility.ShiftCoordinate(secondCoordinate, new HexVectorComponent(hexAxis[1], 1));
                    result.Add(secondCoordinate);
                    // We add this component to shift vector after adding a second vector because we needed the unmodified
                    // shift vector to properly shift the second coordinate.
                    shiftVector.AddComponent(hexAxis[0], 1);
                }
            }
            _hexCoordinatesUtility.ShiftCoordinate(newCoordinate, shiftVector);
            result.Add(newCoordinate);

            return result.ToArray();
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

        private bool IsSeeingThroughHexEdge(int distanceA, int distanceB)
        {
            return distanceA == 0 || distanceB == 0 ||
                   (distanceA / distanceB >= 3) || (distanceB / distanceA >= 3);
        }

        public HexAxis[] GetAxisFromFacing(AvidDirection direction)
        {
            var directionIndex = (byte)direction;
            byte primaryAxisIndex = (byte)((directionIndex + 1) / 2);
            var result = new List<HexAxis> { (HexAxis)primaryAxisIndex };

            if (directionIndex > 0 && directionIndex % 2 == 0) //Hex corner direction
            {
                result.Add((HexAxis)(primaryAxisIndex + 1));
            }
            return result.ToArray();
        }
    }
}
