using FluentAssertions;
using Kinetics.Core;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;
using Kinetics.Core.Interfaces.Calculators;
using KineticToolTests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KineticToolTests
{
    [TestClass]
    public class HexVectorUtilityTests
    {
        public readonly IHexVectorUtility _utilityLibrary = ServiceFactory.Library.HexVectorUtility; 

        [TestMethod]
        public void TestConsolidation()
        {
            var vectorA = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.C, 5),
                SecondaryComponent = new HexVectorComponent(HexAxis.E, 9)
            };
            _utilityLibrary.ConsolidateVector(vectorA);
            TestChecksUtility.CheckComponent(vectorA.PrimaryComponent, HexAxis.D, 5);
            TestChecksUtility.CheckComponent(vectorA.SecondaryComponent, HexAxis.E, 4);

            var vectorB = new RawHexVector();
            vectorB.AddComponent(HexAxis.A, 1);
            vectorB.AddComponent(HexAxis.B, 4);
            vectorB.AddComponent(HexAxis.C, 3);
            vectorB.AddComponent(HexAxis.E, 7);
            vectorB.AddComponent(HexAxis.Up, 2);
            vectorB.AddComponent(HexAxis.Up, 3);
            vectorB.AddComponent(HexAxis.Down, 5);
            var resultB = _utilityLibrary.ConsolidateAndCopyVector(vectorB);
            TestChecksUtility.CheckComponent(resultB.PrimaryComponent, HexAxis.D, 2);
            resultB.SecondaryComponent.Should().Be(HexVectorComponent.Zero);
            resultB.VerticalComponent.Should().Be(HexVectorComponent.Zero);
        }

        [TestMethod]
        public void TestAddition()
        {
            var vectorA = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.A, 9)
            };

            var vectorB = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.D, 7),
                SecondaryComponent = new HexVectorComponent(HexAxis.E, 3),
                VerticalComponent = new HexVectorComponent(HexAxis.Up, 2)
            };

            var result = _utilityLibrary.AddVectors(vectorA, vectorB);

            result.Should().NotBeNull("Result must be valid.");

            result.PrimaryComponent.Direction.Should().Be(HexAxis.F);
            result.PrimaryComponent.Magnitude.Should().Be(2);

            result.SecondaryComponent.Direction.Should().Be(HexAxis.E);
            result.SecondaryComponent.Magnitude.Should().Be(1);

            result.VerticalComponent.Direction.Should().Be(HexAxis.Up);
            result.VerticalComponent.Magnitude.Should().Be(2);
        }

        [TestMethod]
        public void TestSubstraction()
        {
            // Test creating crossing vector. As per example in the rulebook.
            var myVelocity = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.F, 9),
                SecondaryComponent = new HexVectorComponent(HexAxis.E, 5)
            };

            var tartgetVelocity = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.C, 6),
                VerticalComponent = new HexVectorComponent(HexAxis.Up, 4)
            };

            var result = _utilityLibrary.SubstractVectors(tartgetVelocity, myVelocity);

            result.Should().NotBeNull("Result must be valid.");

            result.PrimaryComponent.Direction.Should().Be(HexAxis.C);
            result.PrimaryComponent.Magnitude.Should().Be(15);

            result.SecondaryComponent.Direction.Should().Be(HexAxis.B);
            result.SecondaryComponent.Magnitude.Should().Be(5);

            result.VerticalComponent.Direction.Should().Be(HexAxis.Up);
            result.VerticalComponent.Magnitude.Should().Be(4);
        }

        [TestMethod]
        public void TestVectorDirectionValuesCalculation()
        {
            var vectorA = new RawHexVector();
            vectorA.AddComponent(HexAxis.B, 5);
            vectorA.AddComponent(HexAxis.B, 3);
            vectorA.AddComponent(HexAxis.E, 2);

            int bMagnitude = _utilityLibrary.GetMagnitudeAlongDirection(vectorA, HexAxis.B);
            bMagnitude.Should().Be(8);
            int beMagnitude = _utilityLibrary.GetMagnitudeAlongCardinalDirection(vectorA, HexAxis.B);
            beMagnitude.Should().Be(6);
        }

        [TestMethod]
        public void TestVectorCloning()
        {
            var vectorA = new RawHexVector();
            vectorA.AddComponent(HexAxis.B, 5);
            vectorA.AddComponent(HexAxis.F, 2);

            var hexVector = _utilityLibrary.CloneHexVector<HexVector>(vectorA);
            TestChecksUtility.CheckComponent(hexVector.PrimaryComponent, HexAxis.B, 5);
            TestChecksUtility.CheckComponent(hexVector.SecondaryComponent, HexAxis.F, 2);

            // Cloning does not consolidate vectors. It copies them as they are, so we can't test grid
            // coordinates copy on vector A.
            var vectorB = new RawHexVector();
            vectorB.AddComponent(HexAxis.C, 5);
            vectorB.AddComponent(HexAxis.A, 2);
            var gridCoordinate = _utilityLibrary.CloneHexVector<HexGridCoordinate>(vectorB);
            gridCoordinate.CfCoordinate.Should().Be(5);
            gridCoordinate.DaCoordinate.Should().Be(1);
        }

        [TestMethod]
        public void TestComponentElimination()
        {
            var vectorA = new RawHexVector();
            vectorA.AddComponent(HexAxis.B, 3);
            vectorA.AddComponent(HexAxis.B, 2);
            vectorA.AddComponent(HexAxis.E, 7);
            vectorA.AddComponent(HexAxis.A, 2);
            vectorA.AddComponent(HexAxis.C, 2);

            _utilityLibrary.GetMagnitudeAlongCardinalDirection(vectorA, HexAxis.B).Should().Be(-2);
            _utilityLibrary.GetMagnitudeAlongCardinalDirection(vectorA, HexAxis.A).Should().Be(2);
            _utilityLibrary.GetMagnitudeAlongCardinalDirection(vectorA, HexAxis.C).Should().Be(2);

            _utilityLibrary.EliminateComponentsAlongCardinalDirection(vectorA, HexAxis.B);
            _utilityLibrary.GetMagnitudeAlongCardinalDirection(vectorA, HexAxis.B).Should().Be(0);
            _utilityLibrary.GetMagnitudeAlongCardinalDirection(vectorA, HexAxis.A).Should().Be(2);
            _utilityLibrary.GetMagnitudeAlongCardinalDirection(vectorA, HexAxis.C).Should().Be(2);
        }

        [TestMethod]
        public void TestComponentReplacement()
        {
            var vectorA = new RawHexVector();
            vectorA.AddComponent(HexAxis.B, 3);
            vectorA.AddComponent(HexAxis.B, 2);
            vectorA.AddComponent(HexAxis.E, 7);
            vectorA.AddComponent(HexAxis.A, 2);
            vectorA.AddComponent(HexAxis.C, 2);

            _utilityLibrary.GetMagnitudeAlongDirection(vectorA, HexAxis.B).Should().Be(5);
            _utilityLibrary.GetMagnitudeAlongCardinalDirection(vectorA, HexAxis.B).Should().Be(-2);

            _utilityLibrary.AddOrUpdateVectorDirection(vectorA, 1, HexAxis.B);
            _utilityLibrary.GetMagnitudeAlongDirection(vectorA, HexAxis.B).Should().Be(1);
            _utilityLibrary.GetMagnitudeAlongCardinalDirection(vectorA, HexAxis.B).Should().Be(-6);

            var vectorB = new RawHexVector();
            vectorB.AddComponent(HexAxis.F, 3);

            _utilityLibrary.GetMagnitudeAlongDirection(vectorB, HexAxis.C).Should().Be(0);
            _utilityLibrary.GetMagnitudeAlongCardinalDirection(vectorB, HexAxis.C).Should().Be(-3);

            _utilityLibrary.AddOrUpdateVectorDirection(vectorB, 5, HexAxis.C);
            _utilityLibrary.GetMagnitudeAlongDirection(vectorB, HexAxis.C).Should().Be(5);
            _utilityLibrary.GetMagnitudeAlongCardinalDirection(vectorB, HexAxis.C).Should().Be(2);

        }
    }
}
