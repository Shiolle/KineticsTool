using FluentAssertions;
using Kinetics.Core;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexVectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KineticToolTests
{
    [TestClass]
    public class ReferenceDataTests
    {
        [TestMethod]
        public void TestProjectileDamage()
        {
            int dmg50,
                dmg100,
                dmg200;

            ServiceFactory.Library.ProjectileDamageTable.GetDamages(3f, out  dmg50, out dmg100, out dmg200);
            dmg50.Should().Be(7);
            dmg100.Should().Be(14);
            dmg200.Should().Be(28);

            ServiceFactory.Library.ProjectileDamageTable.GetDamages(3.5f, out  dmg50, out dmg100, out dmg200);
            dmg50.Should().Be(9);
            dmg100.Should().Be(19);
            dmg200.Should().Be(38);

            ServiceFactory.Library.ProjectileDamageTable.GetDamages(6.5f, out  dmg50, out dmg100, out dmg200);
            dmg50.Should().Be(33);
            dmg100.Should().Be(66);
            dmg200.Should().Be(132);

            ServiceFactory.Library.ProjectileDamageTable.GetDamages(17.5f, out  dmg50, out dmg100, out dmg200);
            dmg50.Should().Be(239);
            dmg100.Should().Be(478);
            dmg200.Should().Be(957);

            ServiceFactory.Library.ProjectileDamageTable.GetDamages(20f, out  dmg50, out dmg100, out dmg200);
            dmg50.Should().Be(312);
            dmg100.Should().Be(625);
            dmg200.Should().Be(1250);
        }

        [TestMethod]
        public void RangeAltitudeTableTest()
        {
            var rAlt = ServiceFactory.Library.RangeAltitudeTable;

            var hexVectorA = new HexVector(HexAxis.A, 12, HexAxis.B, 0, HexAxis.Up, 0);
            rAlt.GetDistance(hexVectorA).Should().Be(12);
            rAlt.GetRing(hexVectorA).Should().Be(AvidRing.Ember);

            var hexVectorB = new HexVector(HexAxis.A, 12, HexAxis.B, 0, HexAxis.Up, 4);
            rAlt.GetDistance(hexVectorB).Should().Be(12);
            rAlt.GetRing(hexVectorB).Should().Be(AvidRing.Blue);

            var hexVectorC = new HexVector(HexAxis.A, 12, HexAxis.B, 0, HexAxis.Up, 18);
            rAlt.GetDistance(hexVectorC).Should().Be(21);
            rAlt.GetRing(hexVectorC).Should().Be(AvidRing.Green);

            var hexVectorD = new HexVector(HexAxis.A, 4, HexAxis.B, 0, HexAxis.Up, 23);
            rAlt.GetDistance(hexVectorD).Should().Be(23);
            rAlt.GetRing(hexVectorD).Should().Be(AvidRing.Magenta);

            var hexVectorE = new HexVector(HexAxis.A, 48, HexAxis.B, 0, HexAxis.Up, 10);
            rAlt.GetDistance(hexVectorE).Should().Be(49);
            rAlt.GetRing(hexVectorE).Should().Be(AvidRing.Ember);

            var hexVectorF = new HexVector(HexAxis.A, 48, HexAxis.B, 0, HexAxis.Up, 13);
            rAlt.GetDistance(hexVectorF).Should().Be(49);
            rAlt.GetRing(hexVectorF).Should().Be(AvidRing.Blue);

            var hexVectorG = new HexVector(HexAxis.A, 35, HexAxis.B, 0, HexAxis.Up, 36);
            rAlt.GetDistance(hexVectorG).Should().Be(50);
            rAlt.GetRing(hexVectorG).Should().Be(AvidRing.Green);
        }
    }
}
