using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Kinetics.Storage.Position;
using Kinetics.Storage.Shellstars;

namespace Kinetics.Storage.Situation
{
    [Serializable]
    public class UnitSto
    {
        public UnitSto()
        {
            IncomingProjectiles = new List<ShellstarSto>();
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement]
        public string Position { get; set; }

        [XmlElement]
        public HexVectorSto Velocity { get; set; }

        [XmlArray]
        public List<ShellstarSto> IncomingProjectiles { get; set; }
    }
}