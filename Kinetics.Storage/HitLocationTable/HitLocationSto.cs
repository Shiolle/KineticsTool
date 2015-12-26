using System;
using System.Xml.Serialization;

namespace Kinetics.Storage.HitLocationTable
{
    /// <summary>
    /// This class stores inormation about a single cell in the main git location table.
    /// </summary>
    [Serializable]
    public class HitLocationSto
    {
        [XmlAttribute]
        public string System { get; set; }

        [XmlAttribute]
        public int Armor { get; set; }
    }
}
