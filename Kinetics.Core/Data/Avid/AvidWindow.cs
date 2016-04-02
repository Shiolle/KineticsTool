using Kinetics.Core.Presentation;
using System;
namespace Kinetics.Core.Data.Avid
{
    public class AvidWindow : IEquatable<AvidWindow>
    {
        public AvidWindow()
        {
            AbovePlane = true;
            Direction = AvidDirection.Undefined;
            Ring = AvidRing.Undefined;
        }

        public AvidWindow(AvidDirection direction, AvidRing ring, bool abovePlane)
        {
            Direction = direction;
            Ring = ring;
            AbovePlane = abovePlane || (ring == AvidRing.Ember); //Ember ring is above plane by default.
        }

        public bool AbovePlane { get; set; }
        public AvidDirection Direction { get; set; }
        public AvidRing Ring { get; set; }

        public bool IsCorner
        {
            get { return Direction != AvidDirection.Undefined && (byte)Direction % 2 == 0; }
        }

        #region Equality logic

        public bool Equals(AvidWindow other)
        {
            if (other == null)
            {
                return false;
            }

            // Windows directly up or down are always equal regardless of direction.
            return (Direction == other.Direction || Ring == AvidRing.Magenta) &&
                   Ring == other.Ring &&
                   AbovePlane == other.AbovePlane;
        }

        public override bool Equals(object obj)
        {
            var other = obj as AvidWindow;

            if (other != null)
            {
                return Equals(other);
            }

            var otherStr = obj as string;

            if (!string.IsNullOrEmpty(otherStr))
            {
                return Equals(Parse(otherStr));
            }

            return false;
        }

        public static bool IsNullOrZero(AvidWindow window)
        {
            if (window == null)
            {
                return true;
            }

            if (window.Direction == AvidDirection.Undefined && window.Ring == AvidRing.Undefined)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ((byte)Direction) * 32 + ((byte)Ring) * 16 + (!AbovePlane ? -1 : 1);
        }

        #endregion

        public static AvidWindow Parse(string window)
        {
            return FormattingExtensions.ParseWindow(window);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", FormattingExtensions.AvidDirectionToString(Direction),
                                           FormattingExtensions.AvidRingToNumericFormat(Ring, AbovePlane));
        }
    }
}
