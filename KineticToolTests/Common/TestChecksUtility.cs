using System;
using FluentAssertions;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexGrid;
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

        public static void CheckCoord(HexGridCoordinate testCoordinate, int cfCoord, int daCoord, int altitude, int daVectorMagnitude)
        {
            testCoordinate.CfCoordinate.Should().Be(cfCoord);
            testCoordinate.DaCoordinate.Should().Be(daCoord);
            testCoordinate.Altitude.Should().Be(altitude);

            //Checking vector components
            CheckVectorComponent(testCoordinate, cfCoord, HexAxis.C, HexAxis.F);
            CheckVectorComponent(testCoordinate, daVectorMagnitude, HexAxis.D, HexAxis.A);
            CheckVectorComponent(testCoordinate, altitude, HexAxis.Up, HexAxis.Down);
        }

        private static void CheckVectorComponent(HexGridCoordinate testCoordinate, int expectedMagnitude, HexAxis positiveAxis, HexAxis negativeAxis)
        {
            if (expectedMagnitude == 0)
            {
                return;
            }

            testCoordinate.Components.Should().ContainSingle(hvc => hvc.Magnitude == Math.Abs(expectedMagnitude) &&
                                                                    hvc.Direction == (expectedMagnitude < 0 ? negativeAxis : positiveAxis));
        }
    }
}
