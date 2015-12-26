using Kinetics.Core.Data.Avid;

namespace Kinetics.Core.Interfaces.Formatting
{
    interface IAvidNotationFormatter
    {
        string DirectionToString(AvidDirection direction);
        string RingToString(AvidRing ring);
    }
}
