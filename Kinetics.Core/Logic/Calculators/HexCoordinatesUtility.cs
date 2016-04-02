using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;
using Kinetics.Core.Interfaces.Calculators;

namespace Kinetics.Core.Logic.Calculators
{
    internal class HexCoordinatesUtility : IHexCoordinatesUtility
    {
        private readonly IHexVectorUtility _hexVectorUtility;

        public HexCoordinatesUtility(IHexVectorUtility hexVectorUtility)
        {
            _hexVectorUtility = hexVectorUtility;
        }

        public void ShiftCoordinate(HexGridCoordinate coordinate, HexVectorComponent component)
        {
            coordinate.AddComponent(component);
            _hexVectorUtility.ConsolidateVector(coordinate);
            EliminateBeComponent(coordinate);
        }

        public void ShiftCoordinate(HexGridCoordinate coordinate, HexVector vector)
        {
            coordinate.AddComponents(vector.Components);
            _hexVectorUtility.ConsolidateVector(coordinate);
            EliminateBeComponent(coordinate);
        }

        public void EliminateBeComponent(HexGridCoordinate position)
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
    }
}
