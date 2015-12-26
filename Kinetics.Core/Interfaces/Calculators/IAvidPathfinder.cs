using Kinetics.Core.Data.Avid;

namespace Kinetics.Core.Interfaces.Calculators
{
    internal interface IAvidPathfinder
    {
        AvidPathfindingResult GetShortestPaths(AvidModel model, AvidWindow start, AvidWindow destination, AvidPathingOptions options);
    }
}
