using System;
using System.Collections.Generic;
using System.Linq;

namespace Kinetics.Core.Data.HitLocationTable
{
    public class HitZoneColumn
    {
        public HitZoneColumn(HitZone hitZone, int averageArmor, IEnumerable<HitLocation> locations)
        {
            HitZone = hitZone;
            AverageArmor = averageArmor;
            AssignZones(locations);
        }

        public HitZone HitZone { get; private set; }

        public int AverageArmor { get; private set; }

        public HitLocation[] Elements { get; private set; }

        public HitLocation GetHitLocation(int roll)
        {
            return Elements[roll];
        }

        private void AssignZones(IEnumerable<HitLocation> locations)
        {
            Elements = new HitLocation[10];
            foreach (var hitLocation in locations)
            {
                Elements[hitLocation.Roll] = hitLocation;
            }

            //Check if there are gaps in the table.
            if (Elements.Any(hitLocation => hitLocation == null)) {
                throw new ArgumentException("Failed to initialize hit location table. Elements missing.");
            }
        }
    }
}