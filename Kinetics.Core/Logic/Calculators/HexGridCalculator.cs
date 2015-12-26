using System;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;
using Kinetics.Core.Interfaces.Calculators;

namespace Kinetics.Core.Logic.Calculators
{
    internal class HexGridCalculator : IHexGridCalculator
    {
        private readonly IHexVectorUtility _hexVectorUtility;
        private readonly IAvidCalculator _avidCalculator;

        public HexGridCalculator(IHexVectorUtility hexVectorUtility, IAvidCalculator avidCalculator)
        {
            _hexVectorUtility = hexVectorUtility;
            _avidCalculator = avidCalculator;
        }

        public void Move(HexGridCoordinate position, HexAxis direction, uint distance)
        {
            position.AddComponent(new HexVectorComponent(direction, (int)distance));
            _hexVectorUtility.ConsolidateVector(position);
            EliminateBeComponent(position);
        }

        public AvidVector GetDistance(HexGridCoordinate from, HexGridCoordinate to)
        {
            // Distance between objects on hex grid is the projection of relative positions
            // when these positions are expressed as hex grid vectors.
            return GetRelativeVectorProjection(from, to);
        }

        public AvidVector GetCrossingVector(RawHexVector attackerVelocity, RawHexVector targetVelocity)
        {
            // Crossing vector is only the projection of relative velocity on the avid, when coordinate
            // system is fixed on the current position of the attacker.
            return GetRelativeVectorProjection(attackerVelocity, targetVelocity);
        }

        public AvidVector GetVectorFromBacktracking(RawHexVector observerVelocity, RawHexVector observedVelocity)
        {
            if (observerVelocity.Equals(RawHexVector.Zero) && observedVelocity.Equals(RawHexVector.Zero))
            {
                return new AvidVector(AvidDirection.A, AvidRing.Ember, true, 0);
            }

            var observerModifiedPosition = VectorToCoordinate(_hexVectorUtility.SubstractVectors(RawHexVector.Zero, observedVelocity));
            var observedModifiedPosition = VectorToCoordinate(_hexVectorUtility.SubstractVectors(RawHexVector.Zero, observerVelocity));

            var result = GetDistance(observedModifiedPosition, observerModifiedPosition);
            result.Magnitude = 0; //Both observer and observed are on the same hex.

            return result;
        }

        private AvidVector GetRelativeVectorProjection(RawHexVector observerVector, RawHexVector observedObjectVector)
        {
            var result = _hexVectorUtility.SubstractVectors(observedObjectVector, observerVector);
            return _avidCalculator.ProjectVectorToAvid(result);
        }

        private void EliminateBeComponent(HexGridCoordinate position)
        {
            int beMagnitude = _hexVectorUtility.GetMagnitudeAlongCardinalDirection(position, HexAxis.B);
            if (beMagnitude == 0)
            {
                return;
            }

            position.CfCoordinate = position.CfCoordinate + beMagnitude;
            position.DaCoordinate = position.DaCoordinate - beMagnitude;

            _hexVectorUtility.EliminateComponentsAlongCardinalDirection(position, HexAxis.B);
        }

        private HexGridCoordinate VectorToCoordinate(RawHexVector vector)
        {
            var result = _hexVectorUtility.CloneHexVector<HexGridCoordinate>(vector);
            _hexVectorUtility.ConsolidateVector(result);
            EliminateBeComponent(result);

            return result;
        }
    }
}