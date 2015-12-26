using Kinetics.Core;
using Kinetics.Core.Data.Avid;
using KineticToolTests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KineticToolTests
{
    [TestClass]
    public class AvidCalculatorRotationTests
    {
        [TestMethod]
        public void RotationRingCalculationTest1()
        {
            var axis = new AvidWindow(AvidDirection.A, AvidRing.Ember, true);

            var result = ServiceFactory.Library.AvidCalculator.GetRotationCircleFromNormal(axis);
            TestChecksUtility.CheckAvidWindow(result[0], AvidDirection.Undefined, AvidRing.Magenta, true);
            TestChecksUtility.CheckAvidWindow(result[1], AvidDirection.BC, AvidRing.Green, true);
            TestChecksUtility.CheckAvidWindow(result[2], AvidDirection.BC, AvidRing.Blue, true);
            TestChecksUtility.CheckAvidWindow(result[3], AvidDirection.BC, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[4], AvidDirection.BC, AvidRing.Blue, false);
            TestChecksUtility.CheckAvidWindow(result[5], AvidDirection.BC, AvidRing.Green, false);
            TestChecksUtility.CheckAvidWindow(result[6], AvidDirection.Undefined, AvidRing.Magenta, false);
            TestChecksUtility.CheckAvidWindow(result[7], AvidDirection.EF, AvidRing.Green, false);
            TestChecksUtility.CheckAvidWindow(result[8], AvidDirection.EF, AvidRing.Blue, false);
            TestChecksUtility.CheckAvidWindow(result[9], AvidDirection.EF, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[10], AvidDirection.EF, AvidRing.Blue, true);
            TestChecksUtility.CheckAvidWindow(result[11], AvidDirection.EF, AvidRing.Green, true);
        }

        [TestMethod]
        public void RotationRingCalculationTest2()
        {
            var axis = new AvidWindow(AvidDirection.Undefined, AvidRing.Magenta, true);

            var result = ServiceFactory.Library.AvidCalculator.GetRotationCircleFromNormal(axis);
            TestChecksUtility.CheckAvidWindow(result[0], AvidDirection.EF, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[1], AvidDirection.E, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[2], AvidDirection.DE, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[3], AvidDirection.D, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[4], AvidDirection.CD, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[5], AvidDirection.C, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[6], AvidDirection.BC, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[7], AvidDirection.B, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[8], AvidDirection.AB, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[9], AvidDirection.A, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[10], AvidDirection.FA, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[11], AvidDirection.F, AvidRing.Ember, true);

        }

        [TestMethod]
        public void RotationRingCalculationTest3()
        {
            var axis = new AvidWindow(AvidDirection.Undefined, AvidRing.Magenta, false);

            var result = ServiceFactory.Library.AvidCalculator.GetRotationCircleFromNormal(axis);
            TestChecksUtility.CheckAvidWindow(result[0], AvidDirection.BC, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[1], AvidDirection.C, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[2], AvidDirection.CD, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[3], AvidDirection.D, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[4], AvidDirection.DE, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[5], AvidDirection.E, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[6], AvidDirection.EF, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[7], AvidDirection.F, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[8], AvidDirection.FA, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[9], AvidDirection.A, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[10], AvidDirection.AB, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[11], AvidDirection.B, AvidRing.Ember, true);
        }

        [TestMethod]
        public void RotationRingCalculationTest4()
        {
            var axis = new AvidWindow(AvidDirection.B, AvidRing.Blue, true);

            var result = ServiceFactory.Library.AvidCalculator.GetRotationCircleFromNormal(axis);
            TestChecksUtility.CheckAvidWindow(result[0], AvidDirection.E, AvidRing.Green, true);
            TestChecksUtility.CheckAvidWindow(result[1], AvidDirection.D, AvidRing.Green, true);
            TestChecksUtility.CheckAvidWindow(result[2], AvidDirection.CD, AvidRing.Blue, true);
            TestChecksUtility.CheckAvidWindow(result[3], AvidDirection.CD, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[4], AvidDirection.CD, AvidRing.Blue, false);
            TestChecksUtility.CheckAvidWindow(result[5], AvidDirection.C, AvidRing.Green, false);
            TestChecksUtility.CheckAvidWindow(result[6], AvidDirection.B, AvidRing.Green, false);
            TestChecksUtility.CheckAvidWindow(result[7], AvidDirection.A, AvidRing.Green, false);
            TestChecksUtility.CheckAvidWindow(result[8], AvidDirection.FA, AvidRing.Blue, false);
            TestChecksUtility.CheckAvidWindow(result[9], AvidDirection.FA, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[10], AvidDirection.FA, AvidRing.Blue, true);
            TestChecksUtility.CheckAvidWindow(result[11], AvidDirection.F, AvidRing.Green, true);
        }

        [TestMethod]
        public void RotationRingCalculationTest5()
        {
            var axis = new AvidWindow(AvidDirection.CD, AvidRing.Blue, false);

            var result = ServiceFactory.Library.AvidCalculator.GetRotationCircleFromNormal(axis);
            TestChecksUtility.CheckAvidWindow(result[0], AvidDirection.CD, AvidRing.Green, true);
            TestChecksUtility.CheckAvidWindow(result[1], AvidDirection.DE, AvidRing.Green, true);
            TestChecksUtility.CheckAvidWindow(result[2], AvidDirection.E, AvidRing.Blue, true);
            TestChecksUtility.CheckAvidWindow(result[3], AvidDirection.E, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[4], AvidDirection.E, AvidRing.Blue, false);
            TestChecksUtility.CheckAvidWindow(result[5], AvidDirection.EF, AvidRing.Green, false);
            TestChecksUtility.CheckAvidWindow(result[6], AvidDirection.FA, AvidRing.Green, false);
            TestChecksUtility.CheckAvidWindow(result[7], AvidDirection.AB, AvidRing.Green, false);
            TestChecksUtility.CheckAvidWindow(result[8], AvidDirection.B, AvidRing.Blue, false);
            TestChecksUtility.CheckAvidWindow(result[9], AvidDirection.B, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[10], AvidDirection.B, AvidRing.Blue, true);
            TestChecksUtility.CheckAvidWindow(result[11], AvidDirection.BC, AvidRing.Green, true);
        }

        [TestMethod]
        public void RotationRingCalculationTest6()
        {
            var axis = new AvidWindow(AvidDirection.C, AvidRing.Green, false);

            var result = ServiceFactory.Library.AvidCalculator.GetRotationCircleFromNormal(axis);
            TestChecksUtility.CheckAvidWindow(result[0], AvidDirection.C, AvidRing.Blue, true);
            TestChecksUtility.CheckAvidWindow(result[1], AvidDirection.CD, AvidRing.Blue, true);
            TestChecksUtility.CheckAvidWindow(result[2], AvidDirection.D, AvidRing.Blue, true);
            TestChecksUtility.CheckAvidWindow(result[3], AvidDirection.DE, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[4], AvidDirection.E, AvidRing.Blue, false);
            TestChecksUtility.CheckAvidWindow(result[5], AvidDirection.EF, AvidRing.Blue, false);
            TestChecksUtility.CheckAvidWindow(result[6], AvidDirection.F, AvidRing.Blue, false);
            TestChecksUtility.CheckAvidWindow(result[7], AvidDirection.FA, AvidRing.Blue, false);
            TestChecksUtility.CheckAvidWindow(result[8], AvidDirection.A, AvidRing.Blue, false);
            TestChecksUtility.CheckAvidWindow(result[9], AvidDirection.AB, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[10], AvidDirection.B, AvidRing.Blue, true);
            TestChecksUtility.CheckAvidWindow(result[11], AvidDirection.BC, AvidRing.Blue, true);
        }

        [TestMethod]
        public void RotationRingCalculationTest7()
        {
            var axis = new AvidWindow(AvidDirection.BC, AvidRing.Ember, true);

            var result = ServiceFactory.Library.AvidCalculator.GetRotationCircleFromNormal(axis);
            TestChecksUtility.CheckAvidWindow(result[0], AvidDirection.Undefined, AvidRing.Magenta, true);
            TestChecksUtility.CheckAvidWindow(result[1], AvidDirection.D, AvidRing.Green, true);
            TestChecksUtility.CheckAvidWindow(result[2], AvidDirection.D, AvidRing.Blue, true);
            TestChecksUtility.CheckAvidWindow(result[3], AvidDirection.D, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[4], AvidDirection.D, AvidRing.Blue, false);
            TestChecksUtility.CheckAvidWindow(result[5], AvidDirection.D, AvidRing.Green, false);
            TestChecksUtility.CheckAvidWindow(result[6], AvidDirection.Undefined, AvidRing.Magenta, false);
            TestChecksUtility.CheckAvidWindow(result[7], AvidDirection.A, AvidRing.Green, false);
            TestChecksUtility.CheckAvidWindow(result[8], AvidDirection.A, AvidRing.Blue, false);
            TestChecksUtility.CheckAvidWindow(result[9], AvidDirection.A, AvidRing.Ember, true);
            TestChecksUtility.CheckAvidWindow(result[10], AvidDirection.A, AvidRing.Blue, true);
            TestChecksUtility.CheckAvidWindow(result[11], AvidDirection.A, AvidRing.Green, true);
        }
    }
}
