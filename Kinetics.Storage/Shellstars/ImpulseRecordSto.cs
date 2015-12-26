using System;
using System.Xml.Serialization;

namespace Kinetics.Storage.Shellstars
{
    [Serializable]
    public class ImpulseRecordSto
    {
        [XmlAttribute]
        public string Impulse { get; set; }

        [XmlAttribute]
        public bool IsBurning { get; set; }

        [XmlAttribute]
        public int Range { get; set; }
    }
}