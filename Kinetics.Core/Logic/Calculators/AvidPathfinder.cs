using System;
using System.Collections.Generic;
using System.Linq;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Interfaces.Calculators;

namespace Kinetics.Core.Logic.Calculators
{
    internal class AvidPathfinder : IAvidPathfinder
    {
        public const int MaximumDepth = 6;

        public AvidPathfindingResult GetShortestPaths(AvidModel model, AvidWindow start, AvidWindow destination, AvidPathingOptions options)
        {
            AvidModelWindow startWnd = model.ProjectWindow(start);
            AvidModelWindow destinationWnd = model.ProjectWindow(destination);

            InitializeNodes(model, startWnd, destinationWnd);

            var validLeaves = BuildTree(startWnd, destinationWnd, options);

            var result = new AvidPathfindingResult
            {
                PathExists = validLeaves.Length > 0
            };

            if (result.PathExists)
            {
                result.MinimalDistance = validLeaves[0].NodeDistance;
                result.AllShortestPaths.AddRange(validLeaves.Select(lf => TracePath(lf, startWnd, destinationWnd)));
            }
            return result;
        }

        private void InitializeNodes(AvidModel model, AvidModelWindow start, AvidModelWindow destination)
        {
            model.ClearOperationalData();
            start.MinDistance = 0;

            // We will now exclude all nodes that are on edge, except if such node is either starting point or the destination.
            foreach (var window in model.Windows.Where(window => window.IsOnSpine && !window.Equals(start) && !window.Equals(destination))) {
                window.Excluded = true;
            }
        }

        private LinkedAvidNode[] BuildTree(AvidModelWindow start, AvidModelWindow destination, AvidPathingOptions options)
        {
            var rootNode = new LinkedAvidNode(start, null, null);

            var result = new List<LinkedAvidNode>();
            TraverseDescendants(result, rootNode, destination, options);

            if (result.Count > 0)
            {
                result.Sort((ndA, ndB) => ndA.NodeDistance - ndB.NodeDistance);
                int minimumDistance = result[0].NodeDistance;
                return result.Where(nd => nd.NodeDistance == minimumDistance).ToArray();
            }

            return result.ToArray();
        }

        private void TraverseDescendants(List<LinkedAvidNode> result, LinkedAvidNode node, AvidModelWindow destination, AvidPathingOptions options)
        {
            var validTransitions = node.Window.AdjacentWindows.ToList(); // Create a new list
            validTransitions.AddRange(node.Window.DiagonalWindows);

            foreach (var transition in validTransitions)
            {
                var linkedWindow = transition.GetLinkedNode(node.Window);

                // We encountered an excluded window or we are in a loop!
                if (linkedWindow.Excluded || WindowOccuredInPath(node, linkedWindow))
                {
                    continue;
                }

                if (!CanDoTransition(node, transition, options))
                {
                    continue;
                }

                var newNode = new LinkedAvidNode(linkedWindow, node, transition);

                if (newNode.NodeDistance / 2 > MaximumDepth)
                {
                    continue;
                }

                ProfferNewDistance(linkedWindow, newNode.NodeDistance);

                if (!linkedWindow.Equals(destination))
                {
                    TraverseDescendants(result, newNode, destination, options);
                    continue;
                }

                //New node is destination and we the path is at least minDistance, but not over maxDistance.
                result.Add(newNode);
            }
        }

        private bool CanDoTransition(LinkedAvidNode currentNode, AvidModelLink proposedLink, AvidPathingOptions options)
        {
            if (!proposedLink.IsDiagonal && !proposedLink.IsPolarDiagonal)
            {
                return true;
            }

            //if (currentNode.NodeDepth == 5 && currentNode.Window.Direction == AvidDirection.CD && currentNode.Window.Ring == AvidRing.Blue)
            //{
            //    var result = TracePath(currentNode, null, null);
            //    if (result.PathNodes[2].Window.Direction == AvidDirection.B && result.PathNodes[2].Window.Ring == AvidRing.Blue)
            //    {
                    
            //    }
            //}

            switch (options)
            {
                case AvidPathingOptions.DiagonalTransitionsIgnored:
                    return !proposedLink.IsDiagonal;
                case AvidPathingOptions.DiagonalTransitionsUnlimited:
                    return true;
                case AvidPathingOptions.DiagonalTransitionsTotalLimit:
                    return !proposedLink.IsDiagonal || currentNode.TotalDiagonalTransitions < (currentNode.NodeDistance / 2 + 1) / 6 + 1;
                case AvidPathingOptions.DiagonalTransitionsLimitPerHemisphere:
                    return !proposedLink.IsDiagonal || 
                           proposedLink.IsAbovePlane ?
                               currentNode.UpperHemisphereDiagonalTransitions < (currentNode.DistanceAbovePlane / 2 + 1) / 6 + 1 :
                               currentNode.LowerHemisphereDiagonalTransitions < (currentNode.DistanceBelowPlane / 2 + 1) / 6 + 1;
                case AvidPathingOptions.DiagonalTransitionsLimitWithPolar:
                    if (proposedLink.IsDiagonal)
                    {
                        if (currentNode.TotalDiagonalTransitions > 0 && currentNode.TotalDiagonalTransitions >= 2)
                        {
                            return false;
                        }

                        return proposedLink.IsAbovePlane ?
                               currentNode.UpperHemisphereDiagonalTransitions < (currentNode.DistanceAbovePlane / 2 + 1) / 6 + 1 :
                               currentNode.LowerHemisphereDiagonalTransitions < (currentNode.DistanceBelowPlane / 2 + 1) / 6 + 1;
                    }
                    if (proposedLink.IsPolarDiagonal)
                    {
                        int currentPolarDiagonalTransitionLimit = (currentNode.NodeDistance / 2 + 1) / 6 + 1;
                        // First transition is always available;
                        return currentNode.TotalPolarDiagonalTransitions == 0 ||
                               // the second is only available as the sixth transition if the second diagonal transition has not been taken. 
                               currentNode.TotalPolarDiagonalTransitions < currentPolarDiagonalTransitionLimit &&
                               currentNode.TotalDiagonalTransitions < 2;
                    }
                    return true;
                default:
                    throw new ArgumentException(string.Format("Option not supported: {0}", options), "options");
            }
        }

        private AvidPathInfo TracePath(LinkedAvidNode leaf, AvidModelWindow start, AvidModelWindow destination)
        {
            var pathInfo = new AvidPathInfo(leaf.NodeDistance, start, destination);
            pathInfo.PathNodes.AddRange(leaf.GetBranch());
            pathInfo.PathNodes.Reverse();
            return pathInfo;
        }

        private void ProfferNewDistance(AvidModelWindow window, int newDistance)
        {
            if (window.MinDistance == AvidModelWindow.UndefinedDistance || window.MinDistance > newDistance)
            {
                window.MinDistance = newDistance;
            }
        }

        private static bool WindowOccuredInPath(LinkedAvidNode node, AvidModelWindow window)
        {
            return node.GetBranch().Any(lan => lan.Window.Equals(window));
        }
    }
}
