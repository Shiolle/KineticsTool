using System.Collections.Generic;
using Kinetics.Core.Data.HitLocationTable;

namespace FireControl.Models.Implementation.ShipClassLibrary
{
    internal class ShipClass : HitLocationTable
    {
        public ShipClass(string className, string sourceName, IEnumerable<HitZoneColumn> tableContent)
            : base(className, tableContent)
        {
            Source = sourceName;
        }

        public string Source { get; private set; }

    }
}
