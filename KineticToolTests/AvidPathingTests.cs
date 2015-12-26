using FluentAssertions;
using Kinetics.Core;
using Kinetics.Core.Data.Avid;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KineticToolTests
{
    [TestClass]
    public class AvidPathingTests
    {
        [TestMethod]
        public void AvidPathingTest1()
        {
            var launchWindow = new AvidVector(AvidDirection.D, AvidRing.Blue, true, 27);
            var crossingVector = new AvidVector(AvidDirection.EF, AvidRing.Ember, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(1, launchWindow, crossingVector);
            result.Length.Should().Be(2);
            result.Should().Contain(new AvidWindow(AvidDirection.DE, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.DE, AvidRing.Ember, true));
        }

        [TestMethod]
        public void AvidPathingTest2()
        {
            var launchWindow = new AvidVector(AvidDirection.D, AvidRing.Blue, true, 27);
            var crossingVector = new AvidVector(AvidDirection.F, AvidRing.Ember, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(1, launchWindow, crossingVector);
            result.Length.Should().Be(3);
            result.Should().Contain(new AvidWindow(AvidDirection.D, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.DE, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.DE, AvidRing.Blue, true));
        }

        [TestMethod]
        public void AvidPathingTest3()
        {
            var launchWindow = new AvidVector(AvidDirection.D, AvidRing.Blue, true, 27);
            var crossingVector = new AvidVector(AvidDirection.F, AvidRing.Ember, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(2, launchWindow, crossingVector);
            result.Length.Should().Be(3);
            result.Should().Contain(new AvidWindow(AvidDirection.E, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.E, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.E, AvidRing.Blue, true));
        }

        [TestMethod]
        public void AvidPathingTest4()
        {
            var launchWindow = new AvidVector(AvidDirection.A, AvidRing.Ember, true, 27);
            var crossingVector = new AvidVector(AvidDirection.D, AvidRing.Ember, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(1, launchWindow, crossingVector);
            result.Length.Should().Be(8);
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.AB, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.AB, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.AB, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.FA, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.FA, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.FA, AvidRing.Blue, false));
        }

        [TestMethod]
        public void AvidPathingTest5()
        {
            var launchWindow = new AvidVector(AvidDirection.A, AvidRing.Ember, true, 27);
            var crossingVector = new AvidVector(AvidDirection.D, AvidRing.Ember, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(2, launchWindow, crossingVector);
            result.Length.Should().Be(12);
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Green, false));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Green, false));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Green, false));
        }

        [TestMethod]
        public void AvidPathingTest6()
        {
            var launchWindow = new AvidVector(AvidDirection.Undefined, AvidRing.Magenta, true, 27);
            var crossingVector = new AvidVector(AvidDirection.Undefined, AvidRing.Magenta, false, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(1, launchWindow, crossingVector);
            result.Length.Should().Be(6);

            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.C, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.D, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.E, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Green, true));
        }

        [TestMethod]
        public void AvidPathingTest7()
        {
            var launchWindow = new AvidVector(AvidDirection.Undefined, AvidRing.Magenta, false, 27);
            var crossingVector = new AvidVector(AvidDirection.Undefined, AvidRing.Magenta, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(2, launchWindow, crossingVector);
            result.Length.Should().Be(12);

            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.AB, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.BC, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.C, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.CD, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.D, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.DE, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.E, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.EF, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.FA, AvidRing.Blue, false));
        }

        [TestMethod]
        public void AvidPathingTest8()
        {
            var launchWindow = new AvidVector(AvidDirection.AB, AvidRing.Ember, true, 27);
            var crossingVector = new AvidVector(AvidDirection.DE, AvidRing.Ember, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(1, launchWindow, crossingVector);
            result.Length.Should().Be(8);
            result.Should().Contain(new AvidWindow(AvidDirection.AB, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.AB, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Blue, false));
        }

        [TestMethod]
        public void AvidPathingTest9()
        {
            var launchWindow = new AvidVector(AvidDirection.AB, AvidRing.Ember, true, 27);
            var crossingVector = new AvidVector(AvidDirection.DE, AvidRing.Ember, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(2, launchWindow, crossingVector);
            result.Length.Should().Be(10);
            result.Should().Contain(new AvidWindow(AvidDirection.FA, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.FA, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.FA, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Green, false));
            result.Should().Contain(new AvidWindow(AvidDirection.BC, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.BC, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.BC, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Green, false));
        }

        [TestMethod]
        public void AvidPathingTest10()
        {
            var launchWindow = new AvidVector(AvidDirection.B, AvidRing.Green, true, 27);
            var crossingVector = new AvidVector(AvidDirection.E, AvidRing.Green, false, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(2, launchWindow, crossingVector);
            result.Length.Should().Be(12);
            result.Should().Contain(new AvidWindow(AvidDirection.E, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.D, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.AB, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.BC, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.C, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.FA, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.C, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.CD, AvidRing.Blue, true));
        }

        [TestMethod]
        public void AvidPathingTest11()
        {
            var launchWindow = new AvidVector(AvidDirection.AB, AvidRing.Ember, true, 27);
            var crossingVector = new AvidVector(AvidDirection.DE, AvidRing.Ember, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(2, launchWindow, crossingVector);
            result.Length.Should().Be(10);
            result.Should().Contain(new AvidWindow(AvidDirection.FA, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.FA, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.FA, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Green, false));
            result.Should().Contain(new AvidWindow(AvidDirection.BC, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.BC, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.BC, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Green, false));
        }

        [TestMethod]
        public void AvidPathingTest12()
        {
            var launchWindow = new AvidVector(AvidDirection.A, AvidRing.Blue, true, 27);
            var crossingVector = new AvidVector(AvidDirection.D, AvidRing.Blue, false, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(2, launchWindow, crossingVector);
            result.Length.Should().Be(12);
            result.Should().Contain(new AvidWindow(AvidDirection.Undefined, AvidRing.Magenta, true));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.AB, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.FA, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Ember, true));
        }

        [TestMethod]
        public void AvidPathingTest13()
        {
            var launchWindow = new AvidVector(AvidDirection.DE, AvidRing.Blue, false, 27);
            var crossingVector = new AvidVector(AvidDirection.AB, AvidRing.Blue, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(2, launchWindow, crossingVector);
            result.Length.Should().Be(12);
            result.Should().Contain(new AvidWindow(AvidDirection.Undefined, AvidRing.Magenta, false));
            result.Should().Contain(new AvidWindow(AvidDirection.C, AvidRing.Green, false));
            result.Should().Contain(new AvidWindow(AvidDirection.CD, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.CD, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.CD, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.D, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.DE, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.E, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Green, false));
            result.Should().Contain(new AvidWindow(AvidDirection.EF, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.EF, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.EF, AvidRing.Ember, true));
        }

        [TestMethod]
        public void AvidPathingTest14()
        {
            var launchWindow = new AvidVector(AvidDirection.B, AvidRing.Blue, true, 27);
            var crossingVector = new AvidVector(AvidDirection.E, AvidRing.Blue, false, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(1, launchWindow, crossingVector);
            result.Length.Should().Be(6);
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.AB, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.AB, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.BC, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.BC, AvidRing.Blue, true));
        }

        [TestMethod]
        public void AvidPathingTest15()
        {
            var launchWindow = new AvidVector(AvidDirection.CD, AvidRing.Blue, false, 27);
            var crossingVector = new AvidVector(AvidDirection.FA, AvidRing.Blue, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(1, launchWindow, crossingVector);
            result.Length.Should().Be(7);
            result.Should().Contain(new AvidWindow(AvidDirection.C, AvidRing.Green, false));
            result.Should().Contain(new AvidWindow(AvidDirection.D, AvidRing.Green, false));
            result.Should().Contain(new AvidWindow(AvidDirection.C, AvidRing.Blue, false));
            result.Should().Contain(new AvidWindow(AvidDirection.C, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.CD, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.D, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.D, AvidRing.Blue, false));
        }

        [TestMethod]
        public void AvidPathingTest16()
        {
            var launchWindow = new AvidVector(AvidDirection.FA, AvidRing.Blue, false, 27);
            var crossingVector = new AvidVector(AvidDirection.CD, AvidRing.Ember, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(1, launchWindow, crossingVector);
            result.Length.Should().Be(2);
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Green, false));
            result.Should().Contain(new AvidWindow(AvidDirection.A, AvidRing.Green, false));
        }

        [TestMethod]
        public void AvidPathingTest17()
        {
            var launchWindow = new AvidVector(AvidDirection.FA, AvidRing.Blue, false, 27);
            var crossingVector = new AvidVector(AvidDirection.CD, AvidRing.Ember, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(2, launchWindow, crossingVector);
            result.Length.Should().Be(3);
            result.Should().Contain(new AvidWindow(AvidDirection.Undefined, AvidRing.Magenta, false));
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Green, false));
            result.Should().Contain(new AvidWindow(AvidDirection.E, AvidRing.Green, false));
        }

        [TestMethod]
        public void AvidPathingTest18()
        {
            var launchWindow = new AvidVector(AvidDirection.BC, AvidRing.Ember, true, 27);
            var crossingVector = new AvidVector(AvidDirection.E, AvidRing.Green, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(1, launchWindow, crossingVector);
            result.Length.Should().Be(4);
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.BC, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.C, AvidRing.Blue, true));
            result.Should().Contain(new AvidWindow(AvidDirection.C, AvidRing.Ember, true));
        }

        [TestMethod]
        public void AvidPathingTest19()
        {
            var launchWindow = new AvidVector(AvidDirection.BC, AvidRing.Ember, true, 27);
            var crossingVector = new AvidVector(AvidDirection.E, AvidRing.Green, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(2, launchWindow, crossingVector);
            result.Length.Should().Be(3);
            result.Should().Contain(new AvidWindow(AvidDirection.B, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.C, AvidRing.Green, true));
            result.Should().Contain(new AvidWindow(AvidDirection.CD, AvidRing.Blue, true));
        }

        [TestMethod]
        public void AvidPathingTest20()
        {
            var launchWindow = new AvidVector(AvidDirection.FA, AvidRing.Blue, false, 27);
            var crossingVector = new AvidVector(AvidDirection.EF, AvidRing.Blue, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(1, launchWindow, crossingVector);
            result.Length.Should().Be(1);
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Ember, true));
        }

        [TestMethod]
        public void AvidPathingTest21()
        {
            var launchWindow = new AvidVector(AvidDirection.FA, AvidRing.Blue, false, 27);
            var crossingVector = new AvidVector(AvidDirection.EF, AvidRing.Blue, true, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(2, launchWindow, crossingVector);
            result.Length.Should().Be(1);
            result.Should().Contain(new AvidWindow(AvidDirection.EF, AvidRing.Blue, true));
        }

        [TestMethod]
        public void AvidPathingTest22()
        {
            var launchWindow = new AvidVector(AvidDirection.F, AvidRing.Blue, true, 27);
            var crossingVector = new AvidVector(AvidDirection.F, AvidRing.Blue, false, 20);
            var result = ServiceFactory.Library.AvidCalculator.GetPossibleLaunchWindows(1, launchWindow, crossingVector);
            result.Length.Should().Be(3);
            result.Should().Contain(new AvidWindow(AvidDirection.EF, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.F, AvidRing.Ember, true));
            result.Should().Contain(new AvidWindow(AvidDirection.FA, AvidRing.Ember, true));
        }
    }
}
