using System;
using Kinetics.Core.Interfaces.RefData;

namespace Kinetics.Core.Logic.RefData
{
    internal class ProjectileDamageTable : IProjectileDamageTable
    {
        private const double DamageCoefficient = 1.562d;
        //private const double DamageCoefficient = 1.5517d;

        public void GetDamages(float roc, out int dmg50, out int dmg100, out int dmg200)
        {
            double energyPer50Kg = DamageCoefficient * Math.Pow(roc, 2) / 2d;
            //double energyPer50Kg = GetDamage(roc);

            dmg50 = RoundDamage(energyPer50Kg);
            dmg100 = RoundDamage(energyPer50Kg * 2f);
            dmg200 = RoundDamage(energyPer50Kg * 4f);
        }

        private double GetDamage(float roc)
        {
            double hexPerSegment = 1250; //m/s
            double c = 299792458; // m/s
            double jPerPoint = 50E6;

            double relativeSpeed = roc * hexPerSegment; // m/s

            double kineticEnergy = 50d * Math.Pow(c, 2) * (1/Math.Sqrt(1 - Math.Pow(relativeSpeed / c, 2)) - 1);
            //double kineticEnergy = 50d * Math.Pow(relativeSpeed, 2) / 2d;

            return kineticEnergy / jPerPoint;
        }

        private int RoundDamage(double damage)
        {
            double roundedDamage = Math.Floor(damage);

            if (damage - roundedDamage >= 0.6)
            {
                roundedDamage++;
            }

            return (int)roundedDamage;
        }
    }
}
