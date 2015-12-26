namespace Kinetics.Storage.Configuration.MissileTypes
{
    public class MissileType
    {
        public string TypeName { get; set; }
        public int BurnDuration { get; set; }
        public int Acceleration { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return TypeName;
        }
    }
}
