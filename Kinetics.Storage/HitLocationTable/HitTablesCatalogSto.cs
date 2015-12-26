using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Kinetics.Storage.HitLocationTable
{
    /// <summary>
    /// Stores the list of hit tables for different classes of ships.
    /// </summary>
    [Serializable]
    public class HitTablesCatalogSto
    {
        public HitTablesCatalogSto()
        {
            HitTables = new List<HitLocationTableSto>();
        }

        [XmlAttribute]
        public string CatalogName { get; set; }

        [XmlArray]
        public List<HitLocationTableSto> HitTables { get; set; }
    }
}
