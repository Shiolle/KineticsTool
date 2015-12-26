using System;
using System.Linq;
using System.Text;

namespace Kinetics.Core.Data.HexVectors
{
    /// <summary>
    /// This class represents a consolidated hex vector with primary and secondary components.
    /// </summary>
    public class HexVector : RawHexVector//, IEquatable<HexVector>
    {
        private HexVectorComponent _primaryCompnoent;
        private HexVectorComponent _secondaryComponent;
        private HexVectorComponent _verticalComponent;

        public HexVector() :this(HexAxis.A, 0, HexAxis.B, 0, HexAxis.Up, 0) { }

        public HexVector(
            HexAxis directionA, int magnitudeA,
            HexAxis directionB, int magnitudeB,
            HexAxis directionZ, int magnitudeZ)
            :base()
        {
            if (directionA == HexAxis.Up || directionA == HexAxis.Down || directionA == HexAxis.Undefined)
            {
                throw new ArgumentException(string.Format("DirectionA should be planar and defined, but found {0}", directionA), "directionA");
            }

            if (directionB == HexAxis.Up || directionB == HexAxis.Down || directionB == HexAxis.Undefined)
            {
                throw new ArgumentException(string.Format("DirectionB should be planar and defined, but found {0}", directionB), "directionA");
            }

            if ((directionZ != HexAxis.Up && directionZ != HexAxis.Down) || directionZ == HexAxis.Undefined)
            {
                throw new ArgumentException(string.Format("DirectionA should be vertical and defined, but found {0}", directionZ), "directionA");
            }

            if (magnitudeA < 0 || magnitudeB < 0 || magnitudeZ < 0)
            {
                throw new ArgumentException(string.Format("All magnitudes should be positive (actual magnitudes are: A{0}, B{1}, Z{2}).", magnitudeA, magnitudeB, magnitudeZ));
            }

            if (magnitudeA >= magnitudeB)
            {
                PrimaryComponent = new HexVectorComponent(directionA, magnitudeA);
                SecondaryComponent = new HexVectorComponent(directionB, magnitudeB);
            }
            else
            {
                PrimaryComponent = new HexVectorComponent(directionB, magnitudeB);
                SecondaryComponent = new HexVectorComponent(directionA, magnitudeA);
            }

            VerticalComponent = new HexVectorComponent(directionZ, magnitudeZ);
        }

        public HexVectorComponent PrimaryComponent
        {
            get { return _primaryCompnoent ?? HexVectorComponent.Zero; }

            set { SetComponent(ref _primaryCompnoent, value); }
        }

        public HexVectorComponent SecondaryComponent
        {
            get { return _secondaryComponent ?? HexVectorComponent.Zero; }

            set { SetComponent(ref _secondaryComponent, value); }
        }

        public HexVectorComponent VerticalComponent
        {
            get { return _verticalComponent ?? HexVectorComponent.Zero; }

            set { SetComponent(ref _verticalComponent, value); }
        }

        public int PlanarProjection
        {
            get { return PrimaryComponent.Magnitude + SecondaryComponent.Magnitude; }
        }

        public override string ToString()
        {
            if (this.Equals(Zero))
            {
                return "Zero";
            }

            var builder = new StringBuilder();

            if (!PrimaryComponent.Equals(HexVectorComponent.Zero))
            {
                builder.Append(PrimaryComponent.ToString());
            }
            if (!SecondaryComponent.Equals(HexVectorComponent.Zero))
            {
                builder.Append(SecondaryComponent.ToString());
            }
            if (!VerticalComponent.Equals(HexVectorComponent.Zero))
            {
                builder.Append(VerticalComponent.ToString());
            }

            return builder.ToString();
        }

        protected override void OnComponentsChanged()
        {
            base.OnComponentsChanged();

            _verticalComponent = Components.FirstOrDefault(hvc => hvc.Direction == HexAxis.Up || hvc.Direction == HexAxis.Down);

            var planarComponents = Components.Where(hvc => hvc.Direction != HexAxis.Up && hvc.Direction != HexAxis.Down).ToList();
            planarComponents.Sort((va, vb) => Math.Sign(vb.Magnitude - va.Magnitude));

            _primaryCompnoent = planarComponents.Count > 0 ? planarComponents[0] : null;

            _secondaryComponent = planarComponents.Count > 1 ? planarComponents[1] : null;
        }

        private void SetComponent(ref HexVectorComponent component, HexVectorComponent newValue)
        {
            if (component != null)
            {
                _components.Remove(component);
            }

            component = newValue;

            if (newValue != null)
            {
                _components.Add(newValue);
            }
        }
    }
}
