using FluentAssertions;
using Kinetics.Core;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;
using Kinetics.Core.Interfaces.Calculators;
using KineticToolTests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KineticToolTests
{
    [TestClass]
    public class HexGridCalculatorTests
    {
        private readonly IHexGridCalculator _calculator = ServiceFactory.Library.HexGridCalculator;

        [TestMethod]
        public void CheckCoordinateParsing1()
        {
            TestChecksUtility.CheckCoord(HexGridCoordinate.Parse("-2 1:0"), -2, 1, 0, 2);
        }

        [TestMethod]
        public void CheckCoordinateParsing2()
        {
            TestChecksUtility.CheckCoord(HexGridCoordinate.Parse("3 1:0"), 3, 1, 0, -1);
        }

        [TestMethod]
        public void CheckCoordinateParsing3()
        {
            TestChecksUtility.CheckCoord(HexGridCoordinate.Parse("1 3:-3"), 1, 3, -3, 2);
        }

        [TestMethod]
        public void CheckCoordinateParsing4()
        {
            TestChecksUtility.CheckCoord(HexGridCoordinate.Parse("0 4:3"), 0, 4, 3, 4);
        }

        [TestMethod]
        public void CheckMovement1()
        {
            CheckMovement(3, 3, 0, HexAxis.E, 5, -2, 5, 0, 6);
        }

        [TestMethod]
        public void CheckMovement2()
        {
            CheckMovement(1, 3, 0, HexAxis.C, 2, 3, 4, 0, 2);
        }

        [TestMethod]
        public void CheckMovement3()
        {
            CheckMovement(2, 3, 1, HexAxis.Down, 2, 2, 3, -1, 2);
        }

        [TestMethod]
        public void CheckMovement4()
        {
            CheckMovement(2, 5, 0, HexAxis.B, 4, 6, 3, 0, 0);
        }

        [TestMethod]
        public void CheckMovement5()
        {
            CheckMovement(2, 0, 0, HexAxis.D, 5, 2, 5, 0, 4);
        }

        [TestMethod]
        public void CheckDistance1()
        {
            CheckDistance(HexGridCoordinate.Parse("3 3"), HexGridCoordinate.Parse("1 3"),
                          2, AvidRing.Ember, AvidDirection.EF, true);
        }

        [TestMethod]
        public void CheckDistance2()
        {
            CheckDistance(HexGridCoordinate.Parse("2 5:7"), HexGridCoordinate.Parse("2 5:-3"),
                          10, AvidRing.Magenta, AvidDirection.Undefined, false);
        }

        [TestMethod]
        public void CheckDistance3()
        {
            CheckDistance(HexGridCoordinate.Parse("2 5:6"), HexGridCoordinate.Parse("6 3:2"),
                          5, AvidRing.Blue, AvidDirection.B, false);
        }

        [TestMethod]
        public void CheckDistance4()
        {
            CheckDistance(HexGridCoordinate.Parse("2 5:3"), HexGridCoordinate.Parse("2 5:3"),
                          0, AvidRing.Undefined, AvidDirection.Undefined, true);
        }

        [TestMethod]
        public void CheckDistance5()
        {
            CheckDistance(HexGridCoordinate.Parse("-2 5:1"), HexGridCoordinate.Parse("3 4:11"),
                          11, AvidRing.Green, AvidDirection.B, true);
        }

        [TestMethod]
        public void CheckBacktracking1()
        {
            var observerVelocity = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.A, 6),
                SecondaryComponent = HexVectorComponent.Zero,
                VerticalComponent = HexVectorComponent.Zero
            };

            var observedVelocity = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.B, 5),
                SecondaryComponent = HexVectorComponent.Zero,
                VerticalComponent = HexVectorComponent.Zero
            };
            var window = _calculator.GetVectorFromBacktracking(observerVelocity, observedVelocity);
            TestChecksUtility.CheckAvidWindow(window, AvidDirection.F, AvidRing.Ember, true);
        }

        [TestMethod]
        public void CheckBacktracking2()
        {
            var observerVelocity = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.A, 6),
                SecondaryComponent = new HexVectorComponent(HexAxis.B, 5),
                VerticalComponent = new HexVectorComponent(HexAxis.Up, 7)
            };

            var observedVelocity = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.C, 6),
                SecondaryComponent = new HexVectorComponent(HexAxis.B, 4),
                VerticalComponent = new HexVectorComponent(HexAxis.Up, 1)
            };
            var window = _calculator.GetVectorFromBacktracking(observerVelocity, observedVelocity);
            TestChecksUtility.CheckAvidWindow(window, AvidDirection.FA, AvidRing.Blue, true);
        }

        [TestMethod]
        public void CheckBacktracking3()
        {
            var observerVelocity = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.A, 5),
                SecondaryComponent = new HexVectorComponent(HexAxis.B, 2),
                VerticalComponent = new HexVectorComponent(HexAxis.Down, 1)
            };

            var observedVelocity = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.A, 4),
                SecondaryComponent = new HexVectorComponent(HexAxis.C, 2),
                VerticalComponent = new HexVectorComponent(HexAxis.Up, 5)
            };
            var window = _calculator.GetVectorFromBacktracking(observerVelocity, observedVelocity);
            TestChecksUtility.CheckAvidWindow(window, AvidDirection.A, AvidRing.Green, false);
        }

        [TestMethod]
        public void CheckBacktracking4()
        {
            var observerVelocity = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.A, 1),
                SecondaryComponent = HexVectorComponent.Zero,
                VerticalComponent = new HexVectorComponent(HexAxis.Up, 3)
            };

            var observedVelocity = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.B, 1),
                SecondaryComponent = HexVectorComponent.Zero,
                VerticalComponent = new HexVectorComponent(HexAxis.Up, 14)
            };
            var window = _calculator.GetVectorFromBacktracking(observerVelocity, observedVelocity);
            TestChecksUtility.CheckAvidWindow(window, AvidDirection.Undefined, AvidRing.Magenta, false);
        }

        private void CheckMovement(int initialCf, int initialDa, int initialAlt,
                                   HexAxis moveAxis, uint moveMagnitude,
                                   int finalCf, int finalDa, int finalAlt, int finalDaValue)
        {
            var coordinate = new HexGridCoordinate
            {
                CfCoordinate = initialCf,
                DaCoordinate = initialDa,
                Altitude = initialAlt
            };

            _calculator.Move(coordinate, moveAxis, moveMagnitude);
            TestChecksUtility.CheckCoord(coordinate, finalCf, finalDa, finalAlt, finalDaValue);
        }

        private void CheckDistance(HexGridCoordinate posA, HexGridCoordinate posB, int distance, AvidRing ring, AvidDirection direction, bool isAbovePlane)
        {
            var result = _calculator.GetDistance(posA, posB);
            result.Magnitude.Should().Be(distance);
            result.Ring.Should().Be(ring);
            result.Direction.Should().Be(direction);
            result.AbovePlane.Should().Be(isAbovePlane);
        }
    }
}
