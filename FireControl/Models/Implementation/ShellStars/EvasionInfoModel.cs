using System;
using FireControl.Models.Interfaces.ShellStars;
using Kinetics.Core;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Interfaces.Calculators;

namespace FireControl.Models.Implementation.ShellStars
{
    internal class EvasionInfoModel : IEvasionInfoModel
    {
        private readonly IAvidCalculator _avidCalculator;

        public EvasionInfoModel()
            : this(new AvidWindow(AvidDirection.A, AvidRing.Ember, true), AvidDirection.Undefined)
        { }

        public EvasionInfoModel(AvidWindow impactWindow, AvidDirection referenceDirection)
        {
            _avidCalculator = ServiceFactory.Library.AvidCalculator;
            UpdateDirections(_avidCalculator.GetOppositeWindow(impactWindow), referenceDirection);
        }

        public AvidWindow ImpactWindow { get; private set; }

        public AvidWindow EvasionUp { get; private set; }

        public AvidWindow EvasionDown { get; private set; }

        public AvidWindow EvasionLeft { get; private set; }

        public AvidWindow EvasionRight { get; private set; }

        public void UpdateDirections(AvidWindow launchWindow, AvidDirection referenceDirection)
        {
            if (AvidWindow.IsNullOrZero(launchWindow) ||
                (launchWindow.Ring == AvidRing.Magenta && referenceDirection == AvidDirection.Undefined))
            {
                ResetEvasionInfo();
                return;
            }

            var impactWindow = _avidCalculator.GetOppositeWindow(launchWindow);
            var axis = _avidCalculator.GetOrientationWithoutRoll(impactWindow, referenceDirection);
            ImpactWindow = axis.Nose;
            EvasionUp = axis.Top;
            EvasionDown = axis.Bottom;
            EvasionLeft = axis.Port;
            EvasionRight = axis.Starboard;
            OnEvasionChanged();
        }

        public bool DirectionsDefined
        {
            get { return !AvidWindow.IsNullOrZero(ImpactWindow); }
        }

        public event Action EvasionDirectionsChanged;

        private void ResetEvasionInfo()
        {
            ImpactWindow = null;
            EvasionUp = null;
            EvasionDown = null;
            EvasionLeft = null;
            EvasionRight = null;
            OnEvasionChanged();
        }

        private void OnEvasionChanged()
        {
            var handler = EvasionDirectionsChanged;
            if (handler != null)
            {
                handler();
            }
        }
    }
}
