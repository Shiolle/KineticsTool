using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Kinetics.Storage.HitLocationTable
{
    /// <summary>
    /// Stores information on a single column of a hit location table.
    /// </summary>
    [Serializable]
    public class HitZoneSto
    {
        public HitZoneSto()
        {
            Elements = new List<HitLocationSto>();
        }

        [XmlAttribute]
        public int Roll { get; set; }

        [XmlAttribute]
        public string ZoneName { get; set; }

        [XmlAttribute]
        public int AverageArmor { get; set; }

        [XmlArray]
        public List<HitLocationSto> Elements { get; set; }
    }
}
