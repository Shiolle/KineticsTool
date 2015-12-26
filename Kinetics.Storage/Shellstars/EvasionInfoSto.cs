using System;
using System.Xml.Serialization;

namespace Kinetics.Storage.Shellstars
{
    [Serializable]
    public class EvasionInfoSto
    {
        [XmlElement]
        public string ImpactWindow { get; set; }

        [XmlElement]
        public string EvasionUp { get; set; }

        [XmlAttribute]
        public int FuelSpentUd { get; set; }

        [XmlAttribute]
        public int FuelSpentLr { get; set; }

        [XmlAttribute]
        public int FuelSpentTa { get; set; }
    }
}
