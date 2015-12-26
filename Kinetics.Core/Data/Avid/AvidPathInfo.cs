using System.Collections.Generic;
using Kinetics.Core.Presentation;

namespace Kinetics.Core.Data.Avid
{
    /// <summary>
    /// Contains information on a single path from last node in the PathNodes property to the node represented by Destination property.
    /// </summary>
    internal class AvidPathInfo
    {
        public AvidPathInfo(int distance, AvidModelWindow start, AvidModelWindow destination)
        {
            PathNodes = new List<LinkedAvidNode>();
            Distance = distance;
            Destination = destination;
            Start = start;
        }

        public AvidModelWindow Start { get; private set; }
        public AvidModelWindow Destination { get; private set; }

        public List<LinkedAvidNode> PathNodes { get; private set; }

        public int Distance { get; private set; }

        public AvidPathInfo Clone()
        {
            var result = new AvidPathInfo(Distance, Start, Destination);
            result.PathNodes.AddRange(PathNodes);

            return result;
        }

        /// <summary>
        /// Returns a number of identical paths, of which one is the object is the original path, and others are its clones.
        /// </summary>
        public AvidPathInfo[] Branch(int numberOfCopies)
        {
            var result = new AvidPathInfo[numberOfCopies];
            result[0] = this;
            for (int i = 1; i < numberOfCopies; i++)
            {
                result[i] = Clone();
            }
            return result;
        }

        public override string ToString()
        {
            return FormattingExtensions.AvidPathToString(this);
        }
    }
}
