using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Kinetics.Storage.Situation
{
    [Serializable]
    public class SitRepSto
    {
        public SitRepSto()
        {
            Units = new List<UnitSto>();
        }

        [XmlArray]
        public List<UnitSto> Units { get; set; }
    }
}