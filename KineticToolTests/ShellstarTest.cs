using Kinetics.Core.Data.FiringSolution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kinetics.Core.Interfaces.Calculators;
using Kinetics.Core;
using Kinetics.Core.Data.RefData;
using FluentAssertions;
using Kinetics.Core.Data;

namespace KineticToolTests
{
    [TestClass]
    public class ShellstarTest
    {
        private const int CoilgunAcceleration = 8;

        private readonly IFiringSolutionCalculator _firingSolutionCalc = ServiceFactory.Library.FiringSolutionCalculator;
        private readonly IShellstarBuilder _shellstarBuilder = ServiceFactory.Library.ShellstarBuilder;

        [TestMethod]
        public void CoilgunTest1()
        {
            // Integer RoC. Distance is divisible by RoC.
            var solution = _firingSolutionCalc.CalculateSolution(4, 20, 3, CoilgunAcceleration);
            solution.AimAdjustment.Should().Be(AimAdjustment.Shift1Window);
            solution.RoC.Should().Be(4.0f);

            var shellstarInfo = _shellstarBuilder.BuildShellstarInfo(20, new TurnData(2, 3), solution, null);
            shellstarInfo.RoC.Should().Be(4f);
            shellstarInfo.Dmg50.Should().Be(12);
            shellstarInfo.Dmg100.Should().Be(25);
            shellstarInfo.Dmg200.Should().Be(50);
            shellstarInfo.ImpulseTrack.Count.Should().Be(6);

            shellstarInfo.ImpulseTrack[0].Impulse.Equals("2.3");
            shellstarInfo.ImpulseTrack[0].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[0].Range.Should().Be(20);

            shellstarInfo.ImpulseTrack[1].Impulse.Equals("2.4");
            shellstarInfo.ImpulseTrack[1].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[1].Range.Should().Be(16);

            shellstarInfo.ImpulseTrack[2].Impulse.Equals("2.5");
            shellstarInfo.ImpulseTrack[2].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[2].Range.Should().Be(12);

            shellstarInfo.ImpulseTrack[3].Impulse.Equals("2.6");
            shellstarInfo.ImpulseTrack[3].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[3].Range.Should().Be(8);

            shellstarInfo.ImpulseTrack[4].Impulse.Equals("2.7");
            shellstarInfo.ImpulseTrack[4].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[4].Range.Should().Be(4);

            shellstarInfo.ImpulseTrack[5].Impulse.Equals("2.8");
            shellstarInfo.ImpulseTrack[5].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[5].Range.Should().Be(0);
        }

        [TestMethod]
        public void CoilgunTest2()
        {
            // Integer RoC. Distance is indivisible by RoC.
            var solution = _firingSolutionCalc.CalculateSolution(4, 20, 3, CoilgunAcceleration);
            solution.AimAdjustment.Should().Be(AimAdjustment.Shift1Window);
            solution.RoC.Should().Be(4.0f);

            var shellstarInfo = _shellstarBuilder.BuildShellstarInfo(17, new TurnData(2, 3), solution, null);
            shellstarInfo.RoC.Should().Be(4f);
            shellstarInfo.Dmg50.Should().Be(12);
            shellstarInfo.Dmg100.Should().Be(25);
            shellstarInfo.Dmg200.Should().Be(50);
            shellstarInfo.ImpulseTrack.Count.Should().Be(6);

            shellstarInfo.ImpulseTrack[0].Impulse.Equals("2.3");
            shellstarInfo.ImpulseTrack[0].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[0].Range.Should().Be(17);

            shellstarInfo.ImpulseTrack[1].Impulse.Equals("2.4");
            shellstarInfo.ImpulseTrack[1].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[1].Range.Should().Be(16);

            shellstarInfo.ImpulseTrack[2].Impulse.Equals("2.5");
            shellstarInfo.ImpulseTrack[2].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[2].Range.Should().Be(12);

            shellstarInfo.ImpulseTrack[3].Impulse.Equals("2.6");
            shellstarInfo.ImpulseTrack[3].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[3].Range.Should().Be(8);

            shellstarInfo.ImpulseTrack[4].Impulse.Equals("2.7");
            shellstarInfo.ImpulseTrack[4].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[4].Range.Should().Be(4);

            shellstarInfo.ImpulseTrack[5].Impulse.Equals("2.8");
            shellstarInfo.ImpulseTrack[5].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[5].Range.Should().Be(0);
        }

        [TestMethod]
        public void CoilgunTest3()
        {
            // Fractional RoC, odd number of segments, ends on rounding up.     
            var solution = _firingSolutionCalc.CalculateSolution(6, 12, 2, CoilgunAcceleration);
            solution.AimAdjustment.Should().Be(AimAdjustment.BearingWindow);
            solution.RoC.Should().Be(3.5f);

            var shellstarInfo = _shellstarBuilder.BuildShellstarInfo(13, new TurnData(2, 3), solution, null);
            shellstarInfo.RoC.Should().Be(3.5f);
            shellstarInfo.Dmg50.Should().Be(9);
            shellstarInfo.Dmg100.Should().Be(19);
            shellstarInfo.Dmg200.Should().Be(38);
            shellstarInfo.ImpulseTrack.Count.Should().Be(5);

            shellstarInfo.ImpulseTrack[0].Impulse.Equals("2.3");
            shellstarInfo.ImpulseTrack[0].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[0].Range.Should().Be(13);

            shellstarInfo.ImpulseTrack[1].Impulse.Equals("2.4");
            shellstarInfo.ImpulseTrack[1].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[1].Range.Should().Be(10);

            shellstarInfo.ImpulseTrack[2].Impulse.Equals("2.5");
            shellstarInfo.ImpulseTrack[2].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[2].Range.Should().Be(7);

            shellstarInfo.ImpulseTrack[3].Impulse.Equals("2.6");
            shellstarInfo.ImpulseTrack[3].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[3].Range.Should().Be(3);

            shellstarInfo.ImpulseTrack[4].Impulse.Equals("2.7");
            shellstarInfo.ImpulseTrack[4].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[4].Range.Should().Be(0);
        }

        [TestMethod]
        public void CoilgunTest4()
        {
            // Fractional RoC, even number of segments, ends on rounding down.     
            var solution = _firingSolutionCalc.CalculateSolution(6, 12, 2, CoilgunAcceleration);
            solution.AimAdjustment.Should().Be(AimAdjustment.BearingWindow);
            solution.RoC.Should().Be(3.5f);

            var shellstarInfo = _shellstarBuilder.BuildShellstarInfo(9, new TurnData(2, 3), solution, null);
            shellstarInfo.RoC.Should().Be(3.5f);
            shellstarInfo.Dmg50.Should().Be(9);
            shellstarInfo.Dmg100.Should().Be(19);
            shellstarInfo.Dmg200.Should().Be(38);
            shellstarInfo.ImpulseTrack.Count.Should().Be(4);

            shellstarInfo.ImpulseTrack[0].Impulse.Equals("2.3");
            shellstarInfo.ImpulseTrack[0].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[0].Range.Should().Be(9);

            shellstarInfo.ImpulseTrack[1].Impulse.Equals("2.4");
            shellstarInfo.ImpulseTrack[1].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[1].Range.Should().Be(7);

            shellstarInfo.ImpulseTrack[2].Impulse.Equals("2.5");
            shellstarInfo.ImpulseTrack[2].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[2].Range.Should().Be(3);

            shellstarInfo.ImpulseTrack[3].Impulse.Equals("2.6");
            shellstarInfo.ImpulseTrack[3].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[3].Range.Should().Be(0);
        }

        [TestMethod]
        public void MissileTest1()
        {
            var fireSolution = _firingSolutionCalc.CalculateSolution(6, 11, 2, CoilgunAcceleration);
            var missileAccelData = _firingSolutionCalc.CalculateMissileAcceleration(31, 2, CoilgunAcceleration, fireSolution.AimAdjustment, fireSolution.RoC);
            fireSolution.RoC.Should().Be(3.5f);
            missileAccelData.BurnDistance.Should().Be(5f);
            missileAccelData.ImpulseData.Count.Should().Be(2);

            var shellstarInfo = _shellstarBuilder.BuildShellstarInfo(31, new TurnData(2, 2), fireSolution, missileAccelData);
            shellstarInfo.Dmg50.Should().Be(9);
            shellstarInfo.Dmg100.Should().Be(19);
            shellstarInfo.Dmg200.Should().Be(38);
            shellstarInfo.ImpulseTrack.Count.Should().Be(11);

            shellstarInfo.ImpulseTrack[0].Impulse.Should().Be("2.2");
            shellstarInfo.ImpulseTrack[0].IsBurning.Should().BeTrue();
            shellstarInfo.ImpulseTrack[0].Range.Should().Be(31);

            shellstarInfo.ImpulseTrack[1].Impulse.Should().Be("2.3");
            shellstarInfo.ImpulseTrack[1].IsBurning.Should().BeTrue();
            shellstarInfo.ImpulseTrack[1].Range.Should().Be(29);

            shellstarInfo.ImpulseTrack[2].Impulse.Should().Be("2.4");
            shellstarInfo.ImpulseTrack[2].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[2].Range.Should().Be(26);

            shellstarInfo.ImpulseTrack[3].Impulse.Should().Be("2.5");
            shellstarInfo.ImpulseTrack[3].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[3].Range.Should().Be(24);

            shellstarInfo.ImpulseTrack[9].Impulse.Should().Be("3.3");
            shellstarInfo.ImpulseTrack[9].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[9].Range.Should().Be(3);

            shellstarInfo.ImpulseTrack[10].Impulse.Should().Be("3.4");
            shellstarInfo.ImpulseTrack[10].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[10].Range.Should().Be(0);
        }

        [TestMethod]
        public void MissileTest2()
        {
            var fireSolution = _firingSolutionCalc.CalculateSolution(2, 15, 3, CoilgunAcceleration);
            var missileAccelData = _firingSolutionCalc.CalculateMissileAcceleration(12, 3, CoilgunAcceleration, fireSolution.AimAdjustment, fireSolution.RoC);
            fireSolution.RoC.Should().Be(2f);
            missileAccelData.BurnDistance.Should().Be(2.5f);
            missileAccelData.ImpulseData.Count.Should().Be(3);

            var shellstarInfo = _shellstarBuilder.BuildShellstarInfo(12, new TurnData(2, 8), fireSolution, missileAccelData);
            shellstarInfo.Dmg50.Should().Be(3);
            shellstarInfo.Dmg100.Should().Be(6);
            shellstarInfo.Dmg200.Should().Be(12);
            shellstarInfo.ImpulseTrack.Count.Should().Be(9);

            shellstarInfo.ImpulseTrack[0].Impulse.Should().Be("2.8");
            shellstarInfo.ImpulseTrack[0].IsBurning.Should().BeTrue();
            shellstarInfo.ImpulseTrack[0].Range.Should().Be(12);

            shellstarInfo.ImpulseTrack[1].Impulse.Should().Be("3.1");
            shellstarInfo.ImpulseTrack[1].IsBurning.Should().BeTrue();
            shellstarInfo.ImpulseTrack[1].Range.Should().Be(12);

            shellstarInfo.ImpulseTrack[2].Impulse.Should().Be("3.2");
            shellstarInfo.ImpulseTrack[2].IsBurning.Should().BeTrue();
            shellstarInfo.ImpulseTrack[2].Range.Should().Be(11);

            shellstarInfo.ImpulseTrack[3].Impulse.Should().Be("3.3");
            shellstarInfo.ImpulseTrack[3].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[3].Range.Should().Be(9);

            shellstarInfo.ImpulseTrack[4].Impulse.Should().Be("3.4");
            shellstarInfo.ImpulseTrack[4].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[4].Range.Should().Be(8);

            shellstarInfo.ImpulseTrack[5].Impulse.Should().Be("3.5");
            shellstarInfo.ImpulseTrack[5].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[5].Range.Should().Be(6);

            shellstarInfo.ImpulseTrack[6].Impulse.Should().Be("3.6");
            shellstarInfo.ImpulseTrack[6].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[6].Range.Should().Be(4);

            shellstarInfo.ImpulseTrack[7].Impulse.Should().Be("3.7");
            shellstarInfo.ImpulseTrack[7].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[7].Range.Should().Be(2);

            shellstarInfo.ImpulseTrack[8].Impulse.Should().Be("3.8");
            shellstarInfo.ImpulseTrack[8].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[8].Range.Should().Be(0);
        }

        [TestMethod]
        public void MissileTest3()
        {
            var fireSolution = _firingSolutionCalc.CalculateSolution(5, 13, 2, CoilgunAcceleration);
            var missileAccelData = _firingSolutionCalc.CalculateMissileAcceleration(8, 2, CoilgunAcceleration, fireSolution.AimAdjustment, fireSolution.RoC);
            fireSolution.RoC.Should().Be(3.5f);
            missileAccelData.BurnDistance.Should().Be(5f);
            missileAccelData.ImpulseData.Count.Should().Be(2);

            var shellstarInfo = _shellstarBuilder.BuildShellstarInfo(8, new TurnData(4, 1), fireSolution, missileAccelData);
            shellstarInfo.Dmg50.Should().Be(9);
            shellstarInfo.Dmg100.Should().Be(19);
            shellstarInfo.Dmg200.Should().Be(38);
            shellstarInfo.ImpulseTrack.Count.Should().Be(4);

            shellstarInfo.ImpulseTrack[0].Impulse.Equals("4.1");
            shellstarInfo.ImpulseTrack[0].IsBurning.Should().BeTrue();
            shellstarInfo.ImpulseTrack[0].Range.Should().Be(8);

            shellstarInfo.ImpulseTrack[1].Impulse.Equals("4.2");
            shellstarInfo.ImpulseTrack[1].IsBurning.Should().BeTrue();
            shellstarInfo.ImpulseTrack[1].Range.Should().Be(6);

            shellstarInfo.ImpulseTrack[2].Impulse.Equals("4.3");
            shellstarInfo.ImpulseTrack[2].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[2].Range.Should().Be(3);

            shellstarInfo.ImpulseTrack[3].Impulse.Equals("4.4");
            shellstarInfo.ImpulseTrack[3].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[3].Range.Should().Be(0);
        }

        [TestMethod]
        public void MissileTest4()
        {
            var fireSolution = _firingSolutionCalc.CalculateSolution(1, 19, 4, CoilgunAcceleration);
            var missileAccelData = _firingSolutionCalc.CalculateMissileAcceleration(10, 4, CoilgunAcceleration, fireSolution.AimAdjustment, fireSolution.RoC);
            fireSolution.RoC.Should().Be(2f);
            missileAccelData.BurnDistance.Should().Be(0f);
            missileAccelData.ImpulseData.Count.Should().Be(4);

            var shellstarInfo = _shellstarBuilder.BuildShellstarInfo(10, new TurnData(5, 3), fireSolution, missileAccelData);
            shellstarInfo.Dmg50.Should().Be(3);
            shellstarInfo.Dmg100.Should().Be(6);
            shellstarInfo.Dmg200.Should().Be(12);
            shellstarInfo.ImpulseTrack.Count.Should().Be(10);

            shellstarInfo.ImpulseTrack[0].Impulse.Equals("5.3");
            shellstarInfo.ImpulseTrack[0].IsBurning.Should().BeTrue();
            shellstarInfo.ImpulseTrack[0].Range.Should().Be(10);

            shellstarInfo.ImpulseTrack[1].Impulse.Equals("5.4");
            shellstarInfo.ImpulseTrack[1].IsBurning.Should().BeTrue();
            shellstarInfo.ImpulseTrack[1].Range.Should().Be(11);

            shellstarInfo.ImpulseTrack[2].Impulse.Equals("5.5");
            shellstarInfo.ImpulseTrack[2].IsBurning.Should().BeTrue();
            shellstarInfo.ImpulseTrack[2].Range.Should().Be(12);

            shellstarInfo.ImpulseTrack[3].Impulse.Equals("5.6");
            shellstarInfo.ImpulseTrack[3].IsBurning.Should().BeTrue();
            shellstarInfo.ImpulseTrack[3].Range.Should().Be(11);

            shellstarInfo.ImpulseTrack[4].Impulse.Equals("5.7");
            shellstarInfo.ImpulseTrack[4].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[4].Range.Should().Be(10);

            shellstarInfo.ImpulseTrack[5].Impulse.Equals("5.8");
            shellstarInfo.ImpulseTrack[5].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[5].Range.Should().Be(8);

            shellstarInfo.ImpulseTrack[6].Impulse.Equals("6.1");
            shellstarInfo.ImpulseTrack[6].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[6].Range.Should().Be(6);

            shellstarInfo.ImpulseTrack[7].Impulse.Equals("6.2");
            shellstarInfo.ImpulseTrack[7].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[7].Range.Should().Be(4);

            shellstarInfo.ImpulseTrack[8].Impulse.Equals("6.3");
            shellstarInfo.ImpulseTrack[8].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[8].Range.Should().Be(2);

            shellstarInfo.ImpulseTrack[9].Impulse.Equals("6.4");
            shellstarInfo.ImpulseTrack[9].IsBurning.Should().BeFalse();
            shellstarInfo.ImpulseTrack[9].Range.Should().Be(0);
        }

        [TestMethod]
        public void MissileTest5()
        {
            // Testing sprint missiles.
            const int acceleration = 16;
            var fireSolution = _firingSolutionCalc.CalculateSolution(2, 4, 1, acceleration);
            var missileAccelData = _firingSolutionCalc.CalculateMissileAcceleration(8, 1, acceleration, fireSolution.AimAdjustment, fireSolution.RoC);
            fireSolution.RoC.Should().Be(2f);
            missileAccelData.BurnDistance.Should().Be(0.5f);
            missileAccelData.ImpulseData.Count.Should().Be(1);

            var shellstarInfo = _shellstarBuilder.BuildShellstarInfo(8, new TurnData(5, 3), fireSolution, missileAccelData);
            shellstarInfo.Dmg50.Should().Be(3);
            shellstarInfo.Dmg100.Should().Be(6);
            shellstarInfo.Dmg200.Should().Be(12);
            shellstarInfo.ImpulseTrack.Count.Should().Be(6);

            CheckImpulseData(shellstarInfo.ImpulseTrack[0], "5.3", true, 8);
            CheckImpulseData(shellstarInfo.ImpulseTrack[1], "5.4", false, 7);
            CheckImpulseData(shellstarInfo.ImpulseTrack[2], "5.5", false, 6);
            CheckImpulseData(shellstarInfo.ImpulseTrack[3], "5.6", false, 4);
            CheckImpulseData(shellstarInfo.ImpulseTrack[4], "5.7", false, 2);
            CheckImpulseData(shellstarInfo.ImpulseTrack[5], "5.8", false, 0);
        }

        private void CheckImpulseData(ImpulseTrackElement element, string impulse, bool iIsBurning, int range)
        {
            element.Impulse.ToString().Should().Be(impulse);
            element.IsBurning.Should().Be(iIsBurning);
            element.Range.Should().Be(range);
        }
    }
}
