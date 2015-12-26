using System;
namespace Kinetics.Core.Data.HexVectors
{
    public class HexVectorComponent : IEquatable<HexVectorComponent>
    {
        public HexVectorComponent()
        {
            Direction = HexAxis.A;
            Magnitude = 0;
        }

        public HexVectorComponent(HexAxis direction, int magnitude)
        {
            Direction = direction;
            Magnitude = magnitude;
        }

        public HexAxis Direction { get; set; }
        public int Magnitude { get; set; }

        public static HexVectorComponent Zero
        {
            get
            {
                return new HexVectorComponent(HexAxis.Undefined, 0);
            }
        }

        #region Equality logic

        public bool Equals(HexVectorComponent other)
        {
            if (other == null)
            {
                return this.Equals(Zero);
            }

            // Zero vector components in any direction are equal.
            return (Direction == other.Direction || Magnitude == 0) &&
                   Magnitude == other.Magnitude;
        }

        public override int GetHashCode()
        {
            return ((byte)Direction) + Magnitude * 16;
        }

        public override bool Equals(object obj)
        {
            var other = obj as HexVectorComponent;

            if (other != null)
            {
                return this.Equals(other);
            }

            return false;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}{1}", Direction.ToString(), Magnitude);
        }
    }
}
