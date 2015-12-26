using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Kinetics.Storage.HitLocationTable
{
    [Serializable]
    public class HitDirectionsSto
    {
        public HitDirectionsSto()
        {
            Directions = new List<HullDepthDirectionSto>();
        }

        [XmlArray]
        public List<HullDepthDirectionSto> Directions { get; set; }
    }
}
