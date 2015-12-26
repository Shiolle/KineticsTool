namespace Kinetics.Storage.Configuration.CoilgunTypes
{
    public sealed class CoilgunType
    {
        public string WeaponCode { get; set; }
        public int MvMultiplyer { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return WeaponCode;
        }
    }
}
