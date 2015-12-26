using FluentAssertions;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexVectors;

namespace KineticToolTests.Common
{
    internal class TestChecksUtility
    {
        public static void CheckComponent(HexVectorComponent component, HexAxis expectedDirection, int expectedMagnitude)
        {
            component.Direction.Should().Be(expectedDirection);
            component.Magnitude.Should().Be(expectedMagnitude);
        }

        public static void CheckAvidWindow(AvidWindow window, AvidDirection expectedDirection, AvidRing expectedRing, bool abovePlane)
        {
            window.Direction.Should().Be(expectedDirection);
            window.Ring.Should().Be(expectedRing);
            window.AbovePlane.Should().Be(abovePlane);
        }
    }
}
