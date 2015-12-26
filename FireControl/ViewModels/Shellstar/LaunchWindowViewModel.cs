using System;
using FireControl.Properties;
using Kinetics.Core.Data.Avid;

namespace FireControl.ViewModels.Shellstar
{
    internal class LaunchWindowViewModel
    {
        private readonly AvidWindow _window;

        public LaunchWindowViewModel(AvidWindow window)
        {
            if (window == null)
            {
                throw new ArgumentException(Resources.LaunchWindowViewModel_WindowMustBeValid, "window");
            }

            _window = window;
            ReferenceDirection = AvidDirection.Undefined;
        }

        public AvidWindow Window
        {
            get { return _window; }
        }

        public string Name
        {
            get { return _window.ToString(); }
        }

        public AvidDirection ReferenceDirection { get; set; }

        public bool NeedToSetReferenceDirection
        {
            get { return CanSetReferenceDirection && ReferenceDirection == AvidDirection.Undefined; }
        }

        public bool CanSetReferenceDirection
        {
            get { return _window.Ring == AvidRing.Magenta; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
