using FluentAssertions;
using Kinetics.Core;
using Kinetics.Core.Data.RefData;
using Kinetics.Core.Interfaces.Calculators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KineticToolTests
{
    [TestClass]
    public class FiringSolutionTest
    {
        private readonly IFiringSolutionCalculator _calculator = ServiceFactory.Library.FiringSolutionCalculator;

        [TestMethod]
        public void FiringSolutionCalculationTest()
        {
            var solutionA = _calculator.CalculateSolution(4, 20, 2, 8);
            solutionA.AimAdjustment.Should().Be(AimAdjustment.Shift2Window);
            solutionA.CrossingVector.Should().Be(20);
            solutionA.MuzzleVelocity.Should().Be(16);
            solutionA.ShotGeometryRow.Should().Be(2);
            solutionA.ShotGeometryColumn.Should().Be(2);
            solutionA.CrossingVectorAdjustment.Should().Be(0.5f);
            solutionA.MuzzleVelocityAdjustment.Should().Be(0.5f);
            solutionA.ModifiedCrossingVector.Should().Be(10);
            solutionA.ModifiedMuzzleVelocity.Should().Be(8);
            solutionA.RoCTurn.Should().Be(18);
            solutionA.RoC.Should().Be(2.0f);

            var solutionB = _calculator.CalculateSolution(4, 20, 3, 8);
            solutionB.AimAdjustment.Should().Be(AimAdjustment.Shift1Window);
            solutionB.CrossingVector.Should().Be(20);
            solutionB.MuzzleVelocity.Should().Be(24);
            solutionB.ShotGeometryRow.Should().Be(1);
            solutionB.ShotGeometryColumn.Should().Be(2);
            solutionB.CrossingVectorAdjustment.Should().Be(0.5f);
            solutionB.MuzzleVelocityAdjustment.Should().Be(0.9f);
            solutionB.ModifiedCrossingVector.Should().Be(10);
            solutionB.ModifiedMuzzleVelocity.Should().Be(22);
            solutionB.RoCTurn.Should().Be(32);
            solutionB.RoC.Should().Be(4.0f);

            var solutionC = _calculator.CalculateSolution(2, 20, 2, 8);
            solutionC.AimAdjustment.Should().Be(AimAdjustment.NoShot);
            solutionC.CrossingVector.Should().Be(20);
            solutionC.MuzzleVelocity.Should().Be(16);
            solutionC.ShotGeometryRow.Should().Be(3);
            solutionC.ShotGeometryColumn.Should().Be(4);
            solutionC.CrossingVectorAdjustment.Should().Be(-0.5f);
            solutionC.MuzzleVelocityAdjustment.Should().Be(0f);
            solutionC.ModifiedCrossingVector.Should().Be(-10);
            solutionC.ModifiedMuzzleVelocity.Should().Be(0);
            solutionC.RoCTurn.Should().Be(-10);
            solutionC.RoC.Should().Be(-1f);

            var solutionD = _calculator.CalculateSolution(6, 12, 2, 8);
            solutionD.AimAdjustment.Should().Be(AimAdjustment.BearingWindow);
            solutionD.CrossingVector.Should().Be(12);
            solutionD.MuzzleVelocity.Should().Be(16);
            solutionD.ShotGeometryRow.Should().Be(0);
            solutionD.ShotGeometryColumn.Should().Be(0);
            solutionD.CrossingVectorAdjustment.Should().Be(1f);
            solutionD.MuzzleVelocityAdjustment.Should().Be(1f);
            solutionD.ModifiedCrossingVector.Should().Be(12);
            solutionD.ModifiedMuzzleVelocity.Should().Be(16);
            solutionD.RoCTurn.Should().Be(28);
            solutionD.RoC.Should().Be(3.5f);
        }

        [TestMethod]
        public void MissileAccelerationTest1()
        {
            const int acceleration = 8;
            var fireSolution = _calculator.CalculateSolution(6, 11, 2, acceleration);
            var missileAccelData = _calculator.CalculateMissileAcceleration(31, 2, acceleration, fireSolution.AimAdjustment, fireSolution.RoC);
            fireSolution.RoC.Should().Be(3.5f);
            missileAccelData.ValidLaunch.Should().BeTrue();
            missileAccelData.TotalAcceleration.Should().Be(7f);
            missileAccelData.TotalPositionAdjustment.Should().Be(2f);
            missileAccelData.TableColumn.Should().Be(5);
            missileAccelData.BurnDistance.Should().Be(5f);
            missileAccelData.TargetRange.Should().Be(31);
            missileAccelData.ImpulseData.Count.Should().Be(2);

            missileAccelData.ImpulseData[0].Range.Should().Be(26f);
            missileAccelData.ImpulseData[0].PositionAdjustment.Should().Be(0.5f);

            missileAccelData.ImpulseData[1].Range.Should().Be(29f);
            missileAccelData.ImpulseData[1].PositionAdjustment.Should().Be(1.5f);

            (missileAccelData.ImpulseData[1].Range +
             fireSolution.RoC -
             missileAccelData.ImpulseData[1].PositionAdjustment).Should().Be(missileAccelData.TargetRange);
        }

        [TestMethod]
        public void MissileAccelerationTest2()
        {
            const int acceleration = 8;
            var fireSolution = _calculator.CalculateSolution(2, 15, 3, acceleration);
            var missileAccelData = _calculator.CalculateMissileAcceleration(12, 3, acceleration, fireSolution.AimAdjustment, fireSolution.RoC);
            fireSolution.RoC.Should().Be(2f);
            missileAccelData.ValidLaunch.Should().BeTrue();
            missileAccelData.TotalAcceleration.Should().Be(6f);
            missileAccelData.TotalPositionAdjustment.Should().Be(3.5f);
            missileAccelData.TableColumn.Should().Be(4);
            missileAccelData.BurnDistance.Should().Be(2.5f);
            missileAccelData.TargetRange.Should().Be(12);
            missileAccelData.ImpulseData.Count.Should().Be(3);

            missileAccelData.ImpulseData[0].Range.Should().Be(9.5f);
            missileAccelData.ImpulseData[0].PositionAdjustment.Should().Be(0f);

            missileAccelData.ImpulseData[1].Range.Should().Be(11.5f);
            missileAccelData.ImpulseData[1].PositionAdjustment.Should().Be(1.5f);

            missileAccelData.ImpulseData[2].Range.Should().Be(12f);
            missileAccelData.ImpulseData[2].PositionAdjustment.Should().Be(2f);

            (missileAccelData.ImpulseData[2].Range +
             fireSolution.RoC -
             missileAccelData.ImpulseData[2].PositionAdjustment).Should().Be(missileAccelData.TargetRange);
        }

        [TestMethod]
        public void MissileAccelerationTest3()
        {
            const int acceleration = 8;
            var fireSolution = _calculator.CalculateSolution(5, 13, 3, acceleration);
            var missileAccelData = _calculator.CalculateMissileAcceleration(8, 3, acceleration, fireSolution.AimAdjustment, fireSolution.RoC);
            fireSolution.RoC.Should().Be(4.5f);
            missileAccelData.ValidLaunch.Should().BeFalse();
            missileAccelData.TotalAcceleration.Should().Be(13.5f);
            missileAccelData.TotalPositionAdjustment.Should().Be(4.5f);
            missileAccelData.TableColumn.Should().Be(4);
            missileAccelData.BurnDistance.Should().Be(9f);
            missileAccelData.ImpulseData.Count.Should().Be(0);

            fireSolution = _calculator.CalculateSolution(5, 13, 2, acceleration);
            missileAccelData = _calculator.CalculateMissileAcceleration(8, 2, acceleration, fireSolution.AimAdjustment, fireSolution.RoC);
            fireSolution.RoC.Should().Be(3.5f);
            missileAccelData.ValidLaunch.Should().BeTrue();
            missileAccelData.TotalAcceleration.Should().Be(7f);
            missileAccelData.TotalPositionAdjustment.Should().Be(2f);
            missileAccelData.TableColumn.Should().Be(5);
            missileAccelData.BurnDistance.Should().Be(5f);
            missileAccelData.TargetRange.Should().Be(8);
            missileAccelData.ImpulseData.Count.Should().Be(2);

            missileAccelData.ImpulseData[0].Range.Should().Be(3f);
            missileAccelData.ImpulseData[0].PositionAdjustment.Should().Be(0.5f);

            missileAccelData.ImpulseData[1].Range.Should().Be(6f);
            missileAccelData.ImpulseData[1].PositionAdjustment.Should().Be(1.5f);

            (missileAccelData.ImpulseData[1].Range +
             fireSolution.RoC -
             missileAccelData.ImpulseData[1].PositionAdjustment).Should().Be(missileAccelData.TargetRange);
        }

        [TestMethod]
        public void MissileAccelerationTest4()
        {
            const int acceleration = 8;
            var fireSolution = _calculator.CalculateSolution(1, 19, 4, acceleration);
            var missileAccelData = _calculator.CalculateMissileAcceleration(10, 4, acceleration, fireSolution.AimAdjustment, fireSolution.RoC);
            fireSolution.RoC.Should().Be(2f);
            missileAccelData.ValidLaunch.Should().BeTrue();
            missileAccelData.TotalAcceleration.Should().Be(8f);
            missileAccelData.TotalPositionAdjustment.Should().Be(8f);
            missileAccelData.TableColumn.Should().Be(3);
            missileAccelData.BurnDistance.Should().Be(0f);
            missileAccelData.TargetRange.Should().Be(10);
            missileAccelData.ImpulseData.Count.Should().Be(4);

            missileAccelData.ImpulseData[0].Range.Should().Be(10f);
            missileAccelData.ImpulseData[0].PositionAdjustment.Should().Be(0.5f);

            missileAccelData.ImpulseData[1].Range.Should().Be(11.5f);
            missileAccelData.ImpulseData[1].PositionAdjustment.Should().Be(1.5f);

            missileAccelData.ImpulseData[2].Range.Should().Be(12f);
            missileAccelData.ImpulseData[2].PositionAdjustment.Should().Be(2.5f);

            missileAccelData.ImpulseData[3].Range.Should().Be(11.5f);
            missileAccelData.ImpulseData[3].PositionAdjustment.Should().Be(3.5f);

            (missileAccelData.ImpulseData[3].Range +
             fireSolution.RoC -
             missileAccelData.ImpulseData[3].PositionAdjustment).Should().Be(missileAccelData.TargetRange);
        }

        [TestMethod]
        public void MissileAccelerationTest5()
        {
            // Testing sprint missiles.
            const int acceleration = 16;
            var fireSolution = _calculator.CalculateSolution(2, 4, 1, acceleration);
            var missileAccelData = _calculator.CalculateMissileAcceleration(8, 1, acceleration, fireSolution.AimAdjustment, fireSolution.RoC);
            fireSolution.RoC.Should().Be(2f);
            missileAccelData.ValidLaunch.Should().BeTrue();
            missileAccelData.TableColumn.Should().Be(5);
            missileAccelData.TotalAcceleration.Should().Be(2f);
            missileAccelData.TotalPositionAdjustment.Should().Be(1.5f);
            missileAccelData.BurnDistance.Should().Be(0.5f);
            missileAccelData.TargetRange.Should().Be(8);
            missileAccelData.ImpulseData.Count.Should().Be(1);

            missileAccelData.ImpulseData[0].Range.Should().Be(7.5f);
            missileAccelData.ImpulseData[0].PositionAdjustment.Should().Be(1.5f);
        }
    }
}
