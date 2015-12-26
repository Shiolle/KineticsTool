using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using Kinetics.Storage.Configuration.CoilgunTypes;
using Kinetics.Storage.Configuration.Mapping;
using Kinetics.Storage.Configuration.MissileTypes;

namespace Kinetics.Storage
{
    public class StaticConfigurationController : IStaticConfigurationController
    {
        private const string StartingUnitPositionAppSetting = "UnitsStartingPosition";
        private const string CoilgunTypesSectionName = "coilgunTypes";
        private const string MissileTypesSectionName = "missileTypes";

        public StaticConfigurationController()
        {
            KineticsStorageMappingConfiguration.CreateMappings();
            StartingUnitPosition = ConfigurationManager.AppSettings[StartingUnitPositionAppSetting];

            LoadCoilguns();
            LoadMissiles();
        }

        /// <summary>
        /// Gets starting position where units appear by default when created.
        /// </summary>
        public string StartingUnitPosition { get; private set; }

        /// <summary>
        /// Gets the list of coilgun types.
        /// </summary>
        public CoilgunType[] CoilgunTypes { get; private set; }

        /// <summary>
        /// Get the list of missile types.
        /// </summary>
        public MissileType[] MissileTypes { get; private set; }

        private void LoadCoilguns()
        {
            var coilgunTypesSection = ConfigurationManager.GetSection(CoilgunTypesSectionName) as CoilgunTypesConfigurationSection;

            if (coilgunTypesSection == null)
            {
                throw new Exception("Could not read coilgun configurations.");
            }
            CoilgunTypes = Mapper.Map<IEnumerable<CoilgunTypeConfigurationElement>, IEnumerable<CoilgunType>>(coilgunTypesSection.CoilgunTypes).ToArray();
        }

        private void LoadMissiles()
        {
            var missileTypesSection = ConfigurationManager.GetSection(MissileTypesSectionName) as MissileTypesConfigurationSection;

            if (missileTypesSection == null)
            {
                throw new Exception("Could not read coilgun configurations.");
            }
            MissileTypes = Mapper.Map<IEnumerable<MissileTypeConfigurationElement>, IEnumerable<MissileType>>(missileTypesSection.MissileTypes).ToArray();
        }
    }
}
