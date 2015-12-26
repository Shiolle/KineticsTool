using System;
using System.Configuration;

namespace Kinetics.Storage.Configuration.MissileTypes
{
    internal class MissileTypeConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty(PropertyName.TypeName, IsKey = true, IsRequired = true)]
        public string TypeName
        {
            get
            {
                return (string)this[PropertyName.TypeName];
            }
        }

        [ConfigurationProperty(PropertyName.BurnDuration, IsRequired = true)]
        public int BurnDuration
        {
            get
            {
                return Convert.ToInt32(this[PropertyName.BurnDuration]);
            }
        }

        [ConfigurationProperty(PropertyName.Acceleration, IsRequired = true)]
        public int Acceleration
        {
            get
            {
                return Convert.ToInt32(this[PropertyName.Acceleration]);
            }
        }

        private static class PropertyName
        {
            public const string TypeName = "typeName";
            public const string BurnDuration = "burnDuration";
            public const string Acceleration = "acceleration";
        }
    }
}
