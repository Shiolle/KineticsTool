using System;
using System.Xml.Serialization;

namespace Kinetics.Storage.Position
{
    /// <summary>
    /// Stores a component of a hex vector along a single direction. Also, there can be several components with the same
    /// direction in a hex vector. These are get rid by consolidation, but we don't know when a vector may be saved.
    /// </summary>
    [Serializable]
    public class HexVectorComponentSto
    {
        public HexVectorComponentSto() { }

        public HexVectorComponentSto(string direction, int magnitude)
        {
            Direction = direction;
            Value = magnitude;
        }

        [XmlAttribute]
        public string Direction { get; set; }

        [XmlAttribute]
        public int Value { get; set; }
    }
}