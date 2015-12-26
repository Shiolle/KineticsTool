using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Kinetics.Storage.HitLocationTable
{
    [Serializable]
    public class HitLocationTableSto
    {
        public HitLocationTableSto()
        {
            HitZones = new List<HitZoneSto>();
        }

        [XmlAttribute]
        public string ClassName { get; set; }

        [XmlElement]
        public HitDirectionsSto HitDirections { get; set; }

        [XmlArray]
        public List<HitZoneSto> HitZones { get; set; }
    }
}
