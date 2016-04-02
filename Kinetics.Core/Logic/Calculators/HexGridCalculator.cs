using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;
using Kinetics.Core.Interfaces.Calculators;

namespace Kinetics.Core.Logic.Calculators
{
    internal class HexGridCalculator : IHexGridCalculator
    {
        private readonly IHexVectorUtility _hexVectorUtility;
        private readonly IAvidProjectionCalculator _avidProjectionCalculator;
        private readonly IHexCoordinatesUtility _hexCoordinatesUtility;

        public HexGridCalculator(
            IHexVectorUtility hexVectorUtility,
            IAvidProjectionCalculator avidProjectionCalculator,
            IHexCoordinatesUtility hexCoordinatesUtility)
        {
            _hexVectorUtility = hexVectorUtility;
            _avidProjectionCalculator = avidProjectionCalculator;
            _hexCoordinatesUtility = hexCoordinatesUtility;
        }

        public void Move(HexGridCoordinate position, HexAxis direction, uint distance)
        {
            position.AddComponent(new HexVectorComponent(direction, (int)distance));
            _hexVectorUtility.ConsolidateVector(position);
            _hexCoordinatesUtility.EliminateBeComponent(position);
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
            return _avidProjectionCalculator.ProjectVectorToAvid(result);
        }

        private HexGridCoordinate VectorToCoordinate(RawHexVector vector)
        {
            var result = _hexVectorUtility.CloneHexVector<HexGridCoordinate>(vector);
            _hexVectorUtility.ConsolidateVector(result);
            _hexCoordinatesUtility.EliminateBeComponent(result);

            return result;
        }
    }
}