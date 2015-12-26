using System.Collections.Generic;

namespace Kinetics.Core.Data.HitLocationTable
{
    public class HitLocationTable
    {
        public HitLocationTable(string className, IEnumerable<HitZoneColumn> tableContent)
        {
            ClassName = className;
            AssignHitZones(tableContent);
        }

        public string ClassName { get; private set; }

        public Dictionary<HitZone, HitZoneColumn> HitZones { get; private set; }

        private void AssignHitZones(IEnumerable<HitZoneColumn> columns)
        {
            HitZones = new Dictionary<HitZone, HitZoneColumn>();
            foreach (var column in columns)
            {
                HitZones.Add(column.HitZone, column);
            }
        }
    }
}
