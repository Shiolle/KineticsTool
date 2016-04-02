using Kinetics.Core.Data.HexVectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace KineticToolTests
{
    [TestClass]
    public class HexVectorArithmeticTests
    {
        [TestMethod]
        public void TestComponentEquality()
        {
            var componentA = new HexVectorComponent(HexAxis.A, 5);
            var componentB = new HexVectorComponent(HexAxis.A, 7);
            var componentC = new HexVectorComponent(HexAxis.C, 2);
            var componentD = new HexVectorComponent(HexAxis.A, 5);

            componentA.Equals(componentB).Should().BeFalse();
            componentA.Equals(componentC).Should().BeFalse();
            componentA.Equals(componentD).Should().BeTrue();
            (componentA == componentD).Should().BeFalse();
            componentA.Equals(HexVectorComponent.Zero).Should().BeFalse();
            HexVectorComponent.Zero.Equals(null).Should().BeTrue();
        }

        [TestMethod]
        public void TestVectorEquality()
        {
            var vectorA = new HexVector(HexAxis.A, 2, HexAxis.B, 3, HexAxis.Up, 1);
            var vectorB = new HexVector(HexAxis.A, 2, HexAxis.D, 3, HexAxis.Down, 2);
            var vectorC = new HexVector(HexAxis.A, 2, HexAxis.B, 3, HexAxis.Up, 1);

            vectorA.Equals(vectorB).Should().BeFalse();
            (vectorA == vectorC).Should().BeFalse();
            vectorA.Equals(vectorC).Should().BeTrue();
            vectorA.Equals(RawHexVector.Zero).Should().BeFalse();
            RawHexVector.Zero.Equals(null).Should().BeTrue();
        }
    }
}
