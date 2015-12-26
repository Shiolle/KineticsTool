namespace Kinetics.Core.Data.Avid
{
    public class AvidVector : AvidWindow
    {
        public AvidVector()
            : base()
        { }

        public AvidVector(AvidDirection direction, AvidRing ring, bool abovePlane, int magnitude)
            : base(direction, ring, abovePlane)
        {
            Magnitude = magnitude;
        }

        public int Magnitude { get; set; }

        public static bool IsNullOrZero(AvidVector vector)
        {
            return AvidWindow.IsNullOrZero(vector) || vector.Magnitude == 0;
        }
    }
}
