using Kinetics.Core.Data.HexVectors;
using Kinetics.Core.Interfaces.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kinetics.Core.Logic.Calculators
{
    internal class HexVectorUtility : IHexVectorUtility
    {
        private Dictionary<HexAxis, HexAxis> _oppositeDirections;
        private Dictionary<HexAxis, HexAxis> _orderedPlanarDirections;

        public HexVectorUtility()
        {
            CreateOppositeDirectionsDictionary();
            CreateDirectionOrder();
        }

        public HexVector AddVectors(RawHexVector vectorA, RawHexVector vectorB)
        {
            var rawVector = new RawHexVector();
            rawVector.AddComponents(vectorA.Components);
            rawVector.AddComponents(vectorB.Components);

            return ConsolidateAndCopyVector(rawVector);
        }

        public HexVector SubstractVectors(RawHexVector vectorA, RawHexVector vectorB)
        {
            var invertedSubtrahend = InvertVector(vectorB);

            return AddVectors(vectorA, invertedSubtrahend);
        }

        public RawHexVector InvertVector(RawHexVector initialVector)
        {
            var result = new HexVector();

            var newComponents = initialVector.Components.Select(InvertVectorComponent);
            result.AddComponents(newComponents);

            return result;
        }

        public HexVectorComponent CreateHexVectorComponent(HexAxis positiveDirection, int magnitude)
        {
            return new HexVectorComponent
            {
                Direction = magnitude >= 0 ? positiveDirection : _oppositeDirections[positiveDirection],
                Magnitude = Math.Abs(magnitude)
            };
        }

        public void ConsolidateVector(RawHexVector rawVector)
        {
            List<HexVectorComponent> reductedComponents = rawVector.Components.ToList();

            rawVector.ClearComponents();
            rawVector.AddComponent(ConsolidateCardinalDirection(HexAxis.Up, reductedComponents));

            do
            {
                // Step 1. Consolidate opposing vectors.
                reductedComponents = new List<HexVectorComponent>
                {
                    ConsolidateCardinalDirection(HexAxis.A, reductedComponents),
                    ConsolidateCardinalDirection(HexAxis.B, reductedComponents),
                    ConsolidateCardinalDirection(HexAxis.C, reductedComponents)
                };

                reductedComponents = reductedComponents.Where(av => av.Magnitude > 0).ToList();
                HexVectorComponent fittingVectorA,
                                   fittingVectorB;
                // Step 2. Consolidate vectors 120 degree apart.
                var medianDirection = FindVectors120Apart(reductedComponents, out fittingVectorA, out fittingVectorB);
                if (medianDirection != null && fittingVectorA != null && fittingVectorB != null)
                {
                    int minimumMagnitude = Math.Min(fittingVectorA.Magnitude, fittingVectorB.Magnitude);
                    fittingVectorA.Magnitude -= minimumMagnitude;
                    fittingVectorB.Magnitude -= minimumMagnitude;
                    reductedComponents.Add(new HexVectorComponent(medianDirection.Value, minimumMagnitude)); //This one will be resolved in the next iteration.
                }
            }
            while (reductedComponents.Count > 2 || HasVectors120Apart(reductedComponents));

            rawVector.AddComponents(reductedComponents);
        }

        public HexVector ConsolidateAndCopyVector(RawHexVector rawVector)
        {
            var result = CloneHexVector<HexVector>(rawVector);
            ConsolidateVector(result);

            return result;
        }

        public int GetMagnitudeAlongCardinalDirection(RawHexVector rawVector, HexAxis direction)
        {
            var component = ConsolidateCardinalDirection(direction, rawVector.Components);

            return component.Direction == direction ? component.Magnitude : -component.Magnitude;
        }

        public void EliminateComponentsAlongCardinalDirection(RawHexVector rawVector, HexAxis direction)
        {
            var affectedComponents = rawVector.Components.Where(hvc => hvc.Direction == direction ||
                                                                hvc.Direction == _oppositeDirections[direction]).ToArray();

            foreach (var component in affectedComponents)
            {
                rawVector.RemoveComponent(component);
            }
        }

        public int GetMagnitudeAlongDirection(RawHexVector rawVector, HexAxis direction)
        {
            if (rawVector == null || rawVector.Components == null)
            {
                return 0;
            }

            return GetValueAlongDirection(direction, rawVector.Components);
        }

        public void AddOrUpdateVectorDirection(RawHexVector rawVector, int value, HexAxis direction)
        {
            var affectedComponents = rawVector.Components.Where(hvc => hvc.Direction == direction).ToList();

            if (value > 0)
            {
                if (affectedComponents.Count > 0)
                {
                    var firstComponent = affectedComponents[0];
                    affectedComponents.Remove(firstComponent);
                    firstComponent.Magnitude = value;
                }
                else
                {
                    rawVector.AddComponent(direction, value);
                }
            }

            rawVector.RemoveComponents(affectedComponents);
        }

        public T CloneHexVector<T>(RawHexVector source)
            where T : RawHexVector, new()
        {
            var result = new T();
            result.AddComponents(source.Components);

            return result;
        }

        private int GetValueAlongDirection(HexAxis direction, IEnumerable<HexVectorComponent> components)
        {
            return components.Where(hvc => hvc.Direction.Equals(direction)).Sum(hvc => hvc.Magnitude);
        }

        private HexVectorComponent ConsolidateCardinalDirection(HexAxis direction, IEnumerable<HexVectorComponent> components)
        {
            int speedSum = GetValueAlongDirection(direction, components);
            speedSum -= GetValueAlongDirection(_oppositeDirections[direction], components);

            return new HexVectorComponent
            {
                Direction = speedSum >= 0 ? direction : _oppositeDirections[direction],
                Magnitude = Math.Abs(speedSum)
            };
        }

        private bool HasVectors120Apart(List<HexVectorComponent> components)
        {
            HexVectorComponent velA,
                               velB;
            var medianDirection = FindVectors120Apart(components, out velA, out velB);
            return medianDirection != null && velA != null && velB != null;
        }

        private HexAxis? FindVectors120Apart(List<HexVectorComponent> components, out HexVectorComponent compA, out HexVectorComponent compB)
        {
            compA = null;
            compB = null;
            if (components.Count <= 1)
            {
                return null;
            }

            for (int i = 0; i < components.Count - 1; i++)
            {
                var componentA = components[i];
                for (int j = i + 1; j < components.Count; j++)
                {
                    var componentB = components[j];
                    var vANextDirection = _orderedPlanarDirections[componentA.Direction];
                    var vBNextDirection = _orderedPlanarDirections[componentB.Direction];

                    // Example 1: componentA.Direction = A; componentB.Direction = D. A=>B=>C != D; D=>E=>F != A; continue.
                    // Example 2: componentA.Direction = C; componentB.Direction = E. C=>D=>E == E; E=>F=>A != C; return D.
                    // Example 3: componentA.Direction = A; componentB.Direction = E. A=>B=>C != E; E=>F=>A == A; return F.
                    if (_orderedPlanarDirections[vANextDirection] == componentB.Direction)
                    {
                        compA = componentA;
                        compB = componentB;
                        return vANextDirection;
                    }
                    if (_orderedPlanarDirections[vBNextDirection] == componentA.Direction)
                    {
                        compA = componentA;
                        compB = componentB;
                        return vBNextDirection;
                    }
                }
            }

            return null;
        }

        private HexVectorComponent InvertVectorComponent(HexVectorComponent initialComponent)
        {
            var oppositeDirection = initialComponent.Direction == HexAxis.Undefined ? HexAxis.Undefined : _oppositeDirections[initialComponent.Direction];
            return new HexVectorComponent(oppositeDirection, initialComponent.Magnitude);
        }

        private void CreateOppositeDirectionsDictionary()
        {
            _oppositeDirections = new Dictionary<HexAxis, HexAxis>
            {
                { HexAxis.A, HexAxis.D },
                { HexAxis.B, HexAxis.E },
                { HexAxis.C, HexAxis.F },
                { HexAxis.D, HexAxis.A },
                { HexAxis.E, HexAxis.B },
                { HexAxis.F, HexAxis.C },
                { HexAxis.Up, HexAxis.Down },
                { HexAxis.Down, HexAxis.Up }
            };
        }

        private void CreateDirectionOrder()
        {
            _orderedPlanarDirections = new Dictionary<HexAxis, HexAxis>
            {
                { HexAxis.A, HexAxis.B },
                { HexAxis.B, HexAxis.C },
                { HexAxis.C, HexAxis.D },
                { HexAxis.D, HexAxis.E },
                { HexAxis.E, HexAxis.F },
                { HexAxis.F, HexAxis.A }
            };
        }
    }
}