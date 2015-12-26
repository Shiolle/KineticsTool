using FluentAssertions;
using Kinetics.Core;
using Kinetics.Core.Data.Avid;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KineticToolTests
{
    [TestClass]
    public class AvidDistanceTests
    {
        [TestMethod]
        public void DistanceTest1_OneWindowAdjacent()
        {
            var start = AvidWindow.Parse("A+");
            var destination = AvidWindow.Parse("A/B++");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(1);
        }

        [TestMethod]
        public void DistanceTest2_OneWindowDiagonal()
        {
            var start = AvidWindow.Parse("A");
            var destination = AvidWindow.Parse("A/B+");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(1);
        }

        [TestMethod]
        public void DistanceTest3_TwoWindowsAdjacent()
        {
            var start = AvidWindow.Parse("C+");
            var destination = AvidWindow.Parse("C-");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(2);
        }

        [TestMethod]
        public void DistanceTest4_TwoWindowsDiagonal()
        {
            var start = AvidWindow.Parse("A+");
            var destination = AvidWindow.Parse("B-");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(2);
        }

        [TestMethod]
        public void DistanceTest5_TwoWindowsLevel()
        {
            var start = AvidWindow.Parse("E");
            var destination = AvidWindow.Parse("F");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(2);
        }

        [TestMethod]
        public void DistanceTest6_ThreeWindowsLevel()
        {
            var start = AvidWindow.Parse("E");
            var destination = AvidWindow.Parse("F/A");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(3);
        }

        [TestMethod]
        public void DistanceTest7_ThreeWindowsOneHemisphere()
        {
            var start = AvidWindow.Parse("E");
            var destination = AvidWindow.Parse("A++");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(3);
        }

        [TestMethod]
        public void DistanceTest8_ThreeWindowsDiagonal()
        {
            var start = AvidWindow.Parse("E+");
            var destination = AvidWindow.Parse("F/A-");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(3);
        }

        [TestMethod]
        public void DistanceTest9_FourWindowsLevel()
        {
            var start = AvidWindow.Parse("E/F-");
            var destination = AvidWindow.Parse("B-");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(4);
        }

        [TestMethod]
        public void DistanceTest10_FourWindowsDiagonal()
        {
            var start = AvidWindow.Parse("A+");
            var destination = AvidWindow.Parse("C--");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(4);
        }

        [TestMethod]
        public void DistanceTest11_FourWindowsDiagonal2()
        {
            var start = AvidWindow.Parse("E/F+");
            var destination = AvidWindow.Parse("A/B");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(4);
        }

        [TestMethod]
        public void DistanceTest12_FourWindowsNearVertical()
        {
            var start = AvidWindow.Parse("---");
            var destination = AvidWindow.Parse("F/A+");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(4);
        }

        [TestMethod]
        public void DistanceTest13_FiveWindowsLevel()
        {
            var start = AvidWindow.Parse("D");
            var destination = AvidWindow.Parse("F/A");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(5);
        }

        [TestMethod]
        public void DistanceTest14_FiveWindowsVertical()
        {
            var start = AvidWindow.Parse("D++");
            var destination = AvidWindow.Parse("A-");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(5);
        }

        [TestMethod]
        public void DistanceTest15_FiveWindowsGreenSpine()
        {
            var start = AvidWindow.Parse("C/D++");
            var destination = AvidWindow.Parse("F--");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(5);
        }

        [TestMethod]
        public void DistanceTest16_FiveWindowsBlue()
        {
            var start = AvidWindow.Parse("C/D");
            var destination = AvidWindow.Parse("F/A-");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(5);
        }

        [TestMethod]
        public void DistanceTest17_OppositeMagenta()
        {
            var start = AvidWindow.Parse("+++");
            var destination = AvidWindow.Parse("---");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(6);
        }

        [TestMethod]
        public void DistanceTest18_OppositeGreen()
        {
            var start = AvidWindow.Parse("B++");
            var destination = AvidWindow.Parse("E--");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(6);
        }

        [TestMethod]
        public void DistanceTest19_OppositeGreenSpine()
        {
            var start = AvidWindow.Parse("B/C++");
            var destination = AvidWindow.Parse("E/F--");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(6);
        }

        [TestMethod]
        public void DistanceTest20_OppositeBlueFace()
        {
            var start = AvidWindow.Parse("B+");
            var destination = AvidWindow.Parse("E-");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(6);
        }

        [TestMethod]
        public void DistanceTest21_OppositeBlueCorner()
        {
            var start = AvidWindow.Parse("B/C+");
            var destination = AvidWindow.Parse("E/F-");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(6);
        }

        [TestMethod]
        public void DistanceTest22_OppositeEmberFace()
        {
            var start = AvidWindow.Parse("B");
            var destination = AvidWindow.Parse("E");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(6);
        }

        [TestMethod]
        public void DistanceTest23_OppositeEmberCorner()
        {
            var start = AvidWindow.Parse("C/D");
            var destination = AvidWindow.Parse("F/A");
            int distance = ServiceFactory.Library.AvidCalculator.CountWindows(start, destination);
            distance.Should().Be(6);
        }
    }
}
