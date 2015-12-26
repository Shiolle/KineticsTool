using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Kinetics.Storage.Position
{
    /// <summary>
    /// This class stores information on an AVID window. It may be used in shellstars for evasion direction and impact window,
    /// and also in orientation.
    /// </summary>
    [Serializable]
    public class AvidWindowSto
    {
        [XmlAttribute]
        public string Ring { get; set; }

        [XmlAttribute]
        public string Direction { get; set; }
    }
}
