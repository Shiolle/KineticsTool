using System.Collections.Generic;
using System.Linq;

namespace Kinetics.Core.Data.Avid
{
    /// <summary>
    /// Represents the structure of AVID. For counting, calculating distance, etc.
    /// </summary>
    internal class AvidModel
    {
        public AvidModel()
        {
            Windows = new List<AvidModelWindow>();
            Links = new List<AvidModelLink>();
        }

        public List<AvidModelWindow> Windows { get; private set; }
        public List<AvidModelLink> Links { get; private set; }

        public AvidModelWindow ProjectWindow(AvidWindow window)
        {
            return Windows.Single(amw => amw.Equals(window));
        }

        public void ClearOperationalData()
        {
            Windows.ForEach(wnd => wnd.ClearOperationalData());
        }
    }
}
