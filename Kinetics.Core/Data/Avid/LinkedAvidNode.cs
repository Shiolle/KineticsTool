using System.Collections.Generic;

namespace Kinetics.Core.Data.Avid
{
    /// <summary>
    /// This class represents the node of topological tree that supplementary search algorithm constructs.
    /// </summary>
    internal class LinkedAvidNode
    {
        public LinkedAvidNode(AvidModelWindow window, LinkedAvidNode previousNode, AvidModelLink linkUsed)
        {
            Window = window;
            PreviousNode = previousNode;
            LinkUsed = linkUsed;
            UpdateTransitionsCount();
        }

        public AvidModelWindow Window { get; private set; }

        /// <summary>
        /// Distance from start of the search to this node in the current branch, as opposed to minimum found distance in the window object.
        /// </summary>
        public int NodeDistance { get; private set; }
        public int DistanceAbovePlane { get; private set; }
        public int DistanceBelowPlane { get; private set; }

        public int UpperHemisphereDiagonalTransitions { get; private set; }
        public int LowerHemisphereDiagonalTransitions { get; private set; }
        public int TotalDiagonalTransitions { get; private set; }
        public int TotalPolarDiagonalTransitions { get; private set; }

        public AvidModelLink LinkUsed { get; private set; }
        public LinkedAvidNode PreviousNode { get; private set; }

        public LinkedAvidNode[] GetBranch()
        {
            var result = new List<LinkedAvidNode>();

            LinkedAvidNode currentNode = this;
            while (currentNode != null)
            {
                result.Add(currentNode);
                currentNode = currentNode.PreviousNode;
            }
            return result.ToArray();
        }

        public override string ToString()
        {
            return Window != null ? Window.ToString() : base.ToString();
        }

        private void UpdateTransitionsCount()
        {
            if (PreviousNode == null)
            {
                NodeDistance = 0;
                UpperHemisphereDiagonalTransitions = 0;
                LowerHemisphereDiagonalTransitions = 0;
                TotalPolarDiagonalTransitions = 0;

                DistanceAbovePlane = 0;
                DistanceBelowPlane = 0;
            }
            else
            {
                UpperHemisphereDiagonalTransitions = PreviousNode.UpperHemisphereDiagonalTransitions + (LinkUsed.IsDiagonal && LinkUsed.IsAbovePlane ? 1 : 0);
                LowerHemisphereDiagonalTransitions = PreviousNode.LowerHemisphereDiagonalTransitions + (LinkUsed.IsDiagonal && !LinkUsed.IsAbovePlane ? 1 : 0);
                TotalPolarDiagonalTransitions = PreviousNode.TotalPolarDiagonalTransitions + (LinkUsed.IsPolarDiagonal ? 1 : 0);

                DistanceAbovePlane = PreviousNode.DistanceAbovePlane + (LinkUsed.IsAbovePlane ? LinkUsed.Weight : 0);
                DistanceBelowPlane = PreviousNode.DistanceBelowPlane + (!LinkUsed.IsAbovePlane ? LinkUsed.Weight : 0);
            }

            TotalDiagonalTransitions = UpperHemisphereDiagonalTransitions + LowerHemisphereDiagonalTransitions;
            NodeDistance = DistanceAbovePlane + DistanceBelowPlane;
        }
    }
}
