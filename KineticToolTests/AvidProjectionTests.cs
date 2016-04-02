using FluentAssertions;
using Kinetics.Core;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;
using KineticToolTests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KineticToolTests
{
    [TestClass]
    public class AvidProjectionTests
    {
        [TestMethod]
        public void CornerFacingConversion()
        {
            var vectorA = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.A, 4),
                SecondaryComponent = new HexVectorComponent(HexAxis.B, 2)
            };

            var result = ServiceFactory.Library.AvidProjectionCalculator.ProjectVectorToAvid(vectorA);

            result.Should().NotBeNull();
            result.AbovePlane.Should().BeTrue();
            result.Ring.Should().Be(AvidRing.Ember);
            result.Direction.Should().Be(AvidDirection.AB);
            result.Magnitude.Should().Be(6);
        }

        [TestMethod]
        public void EdgeFacingConversion()
        {
            var vectorA = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.A, 12),
                SecondaryComponent = new HexVectorComponent(HexAxis.F, 2)
            };

            var result = ServiceFactory.Library.AvidProjectionCalculator.ProjectVectorToAvid(vectorA);

            result.Should().NotBeNull();
            result.AbovePlane.Should().BeTrue();
            result.Ring.Should().Be(AvidRing.Ember);
            result.Direction.Should().Be(AvidDirection.A);
            result.Magnitude.Should().Be(14);
        }

        [TestMethod]
        public void BlueRingFacingConversion()
        {
            var vectorA = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.A, 12),
                SecondaryComponent = new HexVectorComponent(HexAxis.F, 2),
                VerticalComponent = new HexVectorComponent(HexAxis.Up, 10)
            };

            var result = ServiceFactory.Library.AvidProjectionCalculator.ProjectVectorToAvid(vectorA);

            result.Should().NotBeNull();
            result.AbovePlane.Should().BeTrue();
            result.Ring.Should().Be(AvidRing.Blue);
            result.Direction.Should().Be(AvidDirection.A);
            result.Magnitude.Should().Be(17);
        }

        [TestMethod]
        public void GreenRingFacingConversion()
        {
            var vectorA = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.A, 7),
                SecondaryComponent = new HexVectorComponent(HexAxis.F, 5),
                VerticalComponent = new HexVectorComponent(HexAxis.Down, 18)
            };

            var result = ServiceFactory.Library.AvidProjectionCalculator.ProjectVectorToAvid(vectorA);

            result.Should().NotBeNull();
            result.AbovePlane.Should().BeFalse();
            result.Ring.Should().Be(AvidRing.Green);
            result.Direction.Should().Be(AvidDirection.FA);
            result.Magnitude.Should().Be(21);
        }

        [TestMethod]
        public void TopFacingConversion()
        {
            var vectorA = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.A, 3),
                SecondaryComponent = new HexVectorComponent(HexAxis.B, 2),
                VerticalComponent = new HexVectorComponent(HexAxis.Up, 27)
            };

            var result = ServiceFactory.Library.AvidProjectionCalculator.ProjectVectorToAvid(vectorA);

            result.Should().NotBeNull();
            result.AbovePlane.Should().BeTrue();
            result.Ring.Should().Be(AvidRing.Magenta);
            result.Direction.Should().Be(AvidDirection.Undefined);
            result.Magnitude.Should().Be(27);
        }

        [TestMethod]
        public void OppositeWindowTest()
        {
            var window = new AvidWindow(AvidDirection.A, AvidRing.Ember, true);

            var result = ServiceFactory.Library.AvidCalculator.GetOppositeWindow(window);
            TestChecksUtility.CheckAvidWindow(result, AvidDirection.D, AvidRing.Ember, true);

            window.Direction = AvidDirection.Undefined;
            window.Ring = AvidRing.Magenta;
            result = ServiceFactory.Library.AvidCalculator.GetOppositeWindow(window);
            TestChecksUtility.CheckAvidWindow(result, AvidDirection.Undefined, AvidRing.Magenta, false);

            window.Direction = AvidDirection.BC;
            window.Ring = AvidRing.Blue;
            window.AbovePlane = false;
            result = ServiceFactory.Library.AvidCalculator.GetOppositeWindow(window);
            TestChecksUtility.CheckAvidWindow(result, AvidDirection.EF, AvidRing.Blue, true);
        }

        [TestMethod]
        public void ReverseProjectionTest1_PlanarHexEdge()
        {
            var position = HexGridCoordinate.Parse("2 3");
            var vector = new AvidVector(AvidDirection.B, AvidRing.Ember, true, 3);

            var result = ServiceFactory.Library.AvidProjectionCalculator.ProvectVectorToMap(vector, position);

            result.Length.Should().Be(1);
            TestChecksUtility.CheckCoord(result[0], 5, 2, 0, -1);
        }

        [TestMethod]
        public void ReverseProjectionTest2_PlanarAmbiguous()
        {
            var position = HexGridCoordinate.Parse("2 3");
            var vector = new AvidVector(AvidDirection.BC, AvidRing.Ember, true, 3);

            var result = ServiceFactory.Library.AvidProjectionCalculator.ProvectVectorToMap(vector, position);

            result.Length.Should().Be(2);
            TestChecksUtility.CheckCoord(result[1], 5, 3, 0, 0);
            TestChecksUtility.CheckCoord(result[0], 5, 4, 0, 1);
        }

        [TestMethod]
        public void ReverseProjectionTest3_PlanarHexCorner()
        {
            var position = HexGridCoordinate.Parse("2 3");
            var vector = new AvidVector(AvidDirection.BC, AvidRing.Ember, true, 4);

            var result = ServiceFactory.Library.AvidProjectionCalculator.ProvectVectorToMap(vector, position);

            result.Length.Should().Be(1);
            TestChecksUtility.CheckCoord(result[0], 6, 3, 0, 0);
        }

        [TestMethod]
        public void ReverseProjectionTest4_Vertical()
        {
            var position = HexGridCoordinate.Parse("2 3: 1");
            var vector = new AvidVector(AvidDirection.B, AvidRing.Magenta, true, 5);

            var result = ServiceFactory.Library.AvidProjectionCalculator.ProvectVectorToMap(vector, position);

            result.Length.Should().Be(1);
            TestChecksUtility.CheckCoord(result[0], 2, 3, 6, 2);
        }

        [TestMethod]
        public void ReverseProjectionTest5_GreenHexEdge()
        {
            var position = HexGridCoordinate.Parse("2 3: 1");
            var vector = new AvidVector(AvidDirection.C, AvidRing.Green, true, 44);

            var result = ServiceFactory.Library.AvidProjectionCalculator.ProvectVectorToMap(vector, position);

            result.Length.Should().Be(1);
            TestChecksUtility.CheckCoord(result[0], 24, 14, 40, 2);
        }

        [TestMethod]
        public void ReverseProjectionTest6_FullCentreline()
        {
            var position = HexGridCoordinate.Parse("0 0");
            var vector = new AvidVector(AvidDirection.D, AvidRing.Blue, true, 1);

            for (int i = 1; i <= 50; i++)
            {
                vector.Magnitude = i;
                var result = ServiceFactory.Library.AvidProjectionCalculator.ProvectVectorToMap(vector, position);

                result.Length.Should().Be(1);
                TestChecksUtility.CheckCoord(result[0], 0, _centraline[0, i - 1], _centraline[1, i - 1], _centraline[0, i - 1]);
            }
        }

        [TestMethod]
        public void ReverseProjectionTest7_GreenHexCorner()
        {
            var position = HexGridCoordinate.Parse("12 11: 3");
            var vector = new AvidVector(AvidDirection.EF, AvidRing.Green, false, 9);

            var result = ServiceFactory.Library.AvidProjectionCalculator.ProvectVectorToMap(vector, position);

            result.Length.Should().Be(2);
            TestChecksUtility.CheckCoord(result[0], 7, 11, -5, 7);
            TestChecksUtility.CheckCoord(result[1], 7, 12, -5, 8);
        }

        private readonly int[,] _centraline =
        {
            { 1, 2, 3, 4, 5, 6, 6, 7, 8, 9, 10, 11, 12, 13, 13, 14, 15, 16, 17, 18, 19, 20, 20, 21, 22, 23, 24, 25, 25, 26, 27, 28, 29, 30, 31, 32, 32, 33, 34, 35, 36, 37, 38, 39, 39, 40, 41, 42, 43, 44 },
            { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5,  6,  6,  7,  7,  8,  8,  9,  9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16, 17, 17, 18, 18, 19, 19, 20, 20, 21, 21, 22, 22, 23, 23, 24, 24, 25, 25 }
        };
    }
}
