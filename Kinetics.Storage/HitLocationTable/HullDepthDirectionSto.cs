using System;
using System.Xml.Serialization;

namespace Kinetics.Storage.HitLocationTable
{
    /// <summary>
    /// Stores information on for hul depth and area for different hit directions.
    /// </summary>
    [Serializable]
    public class HullDepthDirectionSto
    {
        [XmlAttribute]
        public string Direction { get; set; }

        [XmlAttribute]
        public int HullDepth { get; set; }

        [XmlAttribute]
        public int Area { get; set; }
    }
}
