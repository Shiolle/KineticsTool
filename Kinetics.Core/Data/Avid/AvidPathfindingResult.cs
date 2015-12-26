using System.Collections.Generic;

namespace Kinetics.Core.Data.Avid
{
    /// <summary>
    /// Contains pathfinding result of AVID pathfinder.
    /// </summary>
    internal class AvidPathfindingResult
    {
        public AvidPathfindingResult()
        {
            AllShortestPaths = new List<AvidPathInfo>();
        }

        public bool PathExists { get; set; }

        public int MinimalDistance { get; set; }

        public List<AvidPathInfo> AllShortestPaths { get; private set; }
    }
}
