namespace Kinetics.Core.Data.FiringSolution
{
    /// <summary>
    /// Represents a single impulse impulse in an impulse track of the shellstar.
    /// </summary>
    public class ImpulseTrackElement
    {
        public int Range { get; set; }
        public TurnData Impulse { get; set; }
        public bool IsBurning { get; set; }
    }
}
