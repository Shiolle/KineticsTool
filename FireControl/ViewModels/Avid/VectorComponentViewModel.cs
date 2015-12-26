using Kinetics.Core;
using Kinetics.Core.Data.HexVectors;
using Kinetics.Core.Interfaces.Calculators;

namespace FireControl.ViewModels.Avid
{
    internal class VectorComponentViewModel : ViewModelBase
    {
        private readonly HexAxis _direction;

        private readonly IHexVectorUtility _hexVectorUtility;

        public VectorComponentViewModel(HexAxis direction)
        {
            _direction = direction;
            _hexVectorUtility = ServiceFactory.Library.HexVectorUtility;
        }

        public void UpdateVector(RawHexVector vector)
        {
            Value = _hexVectorUtility.GetMagnitudeAlongDirection(vector, _direction);
            HasComponent = Value != 0;
            OnComponentChanged();
        }

        public bool HasComponent { get; private set; }

        public int Value { get; private set; }

        private void OnComponentChanged()
        {
            OnPropertyChanged(Properties.HasComponent);
            OnPropertyChanged(Properties.Value);
        }

        private static class Properties
        {
            public const string HasComponent = "HasComponent";
            public const string Value = "Value";
        }
    }
}
