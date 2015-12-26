using FluentAssertions;
using Kinetics.Core;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexVectors;
using KineticToolTests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KineticToolTests
{
    [TestClass]
    public class AvidCalculatorTests
    {
        [TestMethod]
        public void CornerFacingConversion()
        {
            var vectorA = new HexVector
            {
                PrimaryComponent = new HexVectorComponent(HexAxis.A, 4),
                SecondaryComponent = new HexVectorComponent(HexAxis.B, 2)
            };

            var result = ServiceFactory.Library.AvidCalculator.ProjectVectorToAvid(vectorA);

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

            var result = ServiceFactory.Library.AvidCalculator.ProjectVectorToAvid(vectorA);

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

            var result = ServiceFactory.Library.AvidCalculator.ProjectVectorToAvid(vectorA);

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

            var result = ServiceFactory.Library.AvidCalculator.ProjectVectorToAvid(vectorA);

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

            var result = ServiceFactory.Library.AvidCalculator.ProjectVectorToAvid(vectorA);

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

        private void CheckVector(HexAxis primaryAxis, int primaryMag, HexAxis secondaryAxis, int secondaryMag, HexAxis verticalAxis, int verticalMag)
        {
            var vectorA = new HexVector(primaryAxis, primaryMag, secondaryAxis, secondaryMag, verticalAxis, verticalMag);

            var result = ServiceFactory.Library.AvidCalculator.ProjectVectorToAvid(vectorA);

            result.Should().NotBeNull();
            result.AbovePlane.Should().BeTrue();
            result.Ring.Should().Be(AvidRing.Magenta);
            result.Direction.Should().Be(AvidDirection.Undefined);
            result.Magnitude.Should().Be(27);
        }
    }
}
