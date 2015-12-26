using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Kinetics.Storage.Shellstars
{
    [Serializable]
    public class ShellstarSto
    {
        public ShellstarSto()
        {
            ImpulseTrack = new List<ImpulseRecordSto>();
        }

        [XmlElement]
        public string Tag { get; set; }

        [XmlElement]
        public string SegmentOfLaunch { get; set; }

        [XmlElement]
        public EvasionInfoSto EvasionInfo { get; set; }

        [XmlElement]
        public float Roc { get; set; }

        [XmlElement]
        public int Dmg50 { get; set; }

        [XmlElement]
        public int Dmg100 { get; set; }

        [XmlElement]
        public int Dmg200 { get; set; }

        [XmlArray]
        public List<ImpulseRecordSto> ImpulseTrack { get; set; }
    }
}
