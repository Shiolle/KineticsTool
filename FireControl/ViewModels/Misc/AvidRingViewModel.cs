using Kinetics.Core.Data.Avid;

namespace FireControl.ViewModels.Misc
{
    internal class AvidRingViewModel
    {
        private readonly AvidRing _ring;
        private readonly bool _isAbovePlane;

        public AvidRingViewModel(AvidRing ring, bool isAbovePlane)
        {
            _ring = ring;
            _isAbovePlane = isAbovePlane;
        }

        public AvidRing Ring
        {
            get { return _ring; }
        }

        public bool IsAbovePlane
        {
            get { return _isAbovePlane; }
        }

        public override string ToString()
        {
            var planeSuffix = string.Empty;
            if (Ring != AvidRing.Ember)
            {
                planeSuffix = IsAbovePlane ? "+" : "-";
            }

            return string.Format("{0}{1}", Ring, planeSuffix);
        }
    }
}
