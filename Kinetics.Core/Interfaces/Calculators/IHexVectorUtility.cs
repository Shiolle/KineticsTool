using Kinetics.Core.Data.HexVectors;

namespace Kinetics.Core.Interfaces.Calculators
{
    public interface IHexVectorUtility
    {
        HexVector AddVectors(RawHexVector vectorA, RawHexVector vectorB);

        HexVector SubstractVectors(RawHexVector vectorA, RawHexVector vectorB);

        RawHexVector InvertVector(RawHexVector initialVector);

        void ConsolidateVector(RawHexVector rawVector);

        HexVector ConsolidateAndCopyVector(RawHexVector rawVector);

        HexVectorComponent CreateHexVectorComponent(HexAxis positiveDirection, int magnitude);

        int GetMagnitudeAlongCardinalDirection(RawHexVector rawVector, HexAxis direction);

        void EliminateComponentsAlongCardinalDirection(RawHexVector rawVector, HexAxis direction);

        int GetMagnitudeAlongDirection(RawHexVector rawVector, HexAxis direction);

        void AddOrUpdateVectorDirection(RawHexVector rawVector, int value, HexAxis direction);

        T CloneHexVector<T>(RawHexVector source) where T : RawHexVector, new();
    }
}
