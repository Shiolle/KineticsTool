using Kinetics.Storage.Configuration.CoilgunTypes;
using Kinetics.Storage.Configuration.MissileTypes;

namespace Kinetics.Storage.Configuration.Mapping
{
    internal static class KineticsStorageMappingConfiguration
    {
        private static bool _isConfigured;

        public static void CreateMappings()
        {
            if (_isConfigured)
            {
                return;
            }
            AutoMapper.Mapper.CreateMap<CoilgunTypeConfigurationElement, CoilgunType>();
            AutoMapper.Mapper.CreateMap<MissileTypeConfigurationElement, MissileType>();
            _isConfigured = true;
        }
    }
}
