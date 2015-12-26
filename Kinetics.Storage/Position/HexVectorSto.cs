using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Kinetics.Storage.Position
{
    /// <summary>
    /// This class stores data about vectors on the hex grid. It can also store position.
    /// </summary>
    [Serializable]
    public class HexVectorSto
    {
        public HexVectorSto()
        {
            Components = new List<HexVectorComponentSto>();
        }

        [XmlArray]
        public List<HexVectorComponentSto> Components { get; set; }
    }
}
