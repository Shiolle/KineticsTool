using System.Collections.Generic;
using Kinetics.Core.Data.Avid;

namespace Kinetics.Core.Data.FiringSolution
{
    /// <summary>
    /// A partial inof of the shellstar. A final product calculated from firing solution and selected impact window.
    /// </summary>
    public class ShellstarInfo
    {
        public ShellstarInfo()
        {
            ImpulseTrack = new List<ImpulseTrackElement>();
        }

        public float RoC { get; set; }

        public int Dmg50 { get; set; }
        public int Dmg100 { get; set; }
        public int Dmg200 { get; set; }

        public AvidWindow ImapctWindow { get; set; }
        public AvidWindow EvasionUp { get; set; }
        public AvidWindow EvasionDown { get; set; }
        public AvidWindow EvasionLeft { get; set; }
        public AvidWindow EvasionRight { get; set; }

        public List<ImpulseTrackElement> ImpulseTrack { get; private set; }
    }
}
