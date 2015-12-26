using System;
using Kinetics.Core.Presentation;

namespace Kinetics.Core.Data.Avid
{
    internal class AvidModelLink
    {
        public AvidModelLink(AvidModelWindow endpointA, AvidModelWindow endpointB, int weight, bool isDiagonal, bool isPolarDiagonal)
        {
            if (endpointA == null || endpointB == null)
            {
                throw new ArgumentNullException();
            }

            if (endpointA.Equals(endpointB))
            {
                throw new ArgumentException("A window can't be connected to itself.");
            }

            if (weight < 0)
            {
                throw new ArgumentException("Link weight should be non-negative.", "weight");
            }

            EndpointA = endpointA;
            EndpointB = endpointB;
            Weight = weight;
            IsDiagonal = isDiagonal;
            IsPolarDiagonal = isPolarDiagonal;
            AddLinkToEndpoints();
        }

        public AvidModelWindow EndpointA { get; private set; }
        public AvidModelWindow EndpointB { get; private set; }

        public int Weight { get; private set; }
        public bool IsDiagonal { get; private set; }
        public bool IsPolarDiagonal { get; private set; }

        public bool IsAbovePlane
        {
            get { return EndpointA.AbovePlane && EndpointB.AbovePlane; }
        }

        public AvidModelWindow GetLinkedNode(AvidModelWindow currentNode)
        {
            return EndpointA.Equals(currentNode) ? EndpointB : EndpointA;
        }

        private void AddLinkToEndpoints()
        {
            if (IsDiagonal)
            {
                EndpointA.DiagonalWindows.Add(this);
                EndpointB.DiagonalWindows.Add(this);
            }
            else
            {
                EndpointA.AdjacentWindows.Add(this);
                EndpointB.AdjacentWindows.Add(this);
            }
        }

        public override string ToString()
        {
            return FormattingExtensions.AvidLinkToString(this);
        }
    }
}
