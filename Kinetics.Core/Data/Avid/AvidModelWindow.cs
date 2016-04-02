using System;
using System.Collections.Generic;

namespace Kinetics.Core.Data.Avid
{
    internal class AvidModelWindow : AvidWindow, IEquatable<AvidModelWindow>
    {
        public const int UndefinedDistance = -1;

        public AvidModelWindow(AvidDirection direction, AvidRing ring, bool abovePlane)
            : base(direction, ring, abovePlane)
        {
            AdjacentWindows = new List<AvidModelLink>();
            DiagonalWindows = new List<AvidModelLink>();
            ClearOperationalData();
        }

        public List<AvidModelLink> AdjacentWindows { get; private set; }
        public List<AvidModelLink> DiagonalWindows { get; private set; }

        public bool IsOnSpine
        {
            get
            {
                return Ring == AvidRing.Green &&
                       IsCorner;
            }
        }

        public bool Excluded { get; set; }
        public int MinDistance { get; set; }

        public void ClearOperationalData()
        {
            Excluded = false;
            MinDistance = UndefinedDistance;
        }

        public bool Equals(AvidModelWindow other)
        {
            if (other == null)
            {
                return false;
            }

            return Direction == other.Direction &&
                   Ring == other.Ring &&
                   AbovePlane == other.AbovePlane;
        }
    }
}
