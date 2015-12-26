using System;
using System.Linq;
using FireControl.Properties;
using FireControl.ViewModels.DataContexts;
using Kinetics.Core.Data.Avid;

namespace FireControl.ViewModels.Misc
{
    internal class WindowSelectionViewModel : ViewModelBase, IDialogWindowViewModel<AvidWindow>
    {
        private readonly AvidDirection[] _directions;
        private readonly AvidRingViewModel[] _rings;

        private AvidWindow _window;
        private bool _canEditDirection;
        private bool _canEditRing;
        private bool _directionLocalLock;

        public WindowSelectionViewModel()
        {
            _directions = Enum.GetValues(typeof(AvidDirection)).Cast<AvidDirection>().Except(new[] { AvidDirection.Undefined }).ToArray();

            _rings = new[]
            {
                new AvidRingViewModel(AvidRing.Magenta, true),
                new AvidRingViewModel(AvidRing.Green, true),
                new AvidRingViewModel(AvidRing.Blue, true),
                new AvidRingViewModel(AvidRing.Ember, true),
                new AvidRingViewModel(AvidRing.Blue, false),
                new AvidRingViewModel(AvidRing.Green, false),
                new AvidRingViewModel(AvidRing.Magenta, false)
            };

            Caption = "Select AVID window";
            Message = string.Empty;

            _canEditDirection = false;
            _canEditRing = false;
            _directionLocalLock = false;

            Result = null;
            IsConfirmed = false;
        }

        public void Initialize(object context)
        {
            Result = null;
            IsConfirmed = false;

            var windowSelectionContext = context as WindowSelectionContext;

            if (windowSelectionContext == null)
            {
                throw new ArgumentException(Resources.WindowSelectionViewModel_Unexpected_context_type, "context");
            }

            _window = windowSelectionContext.InitialWindow ?? new AvidWindow(AvidDirection.A, AvidRing.Ember, true);

            Caption = windowSelectionContext.Caption;
            Message = windowSelectionContext.Message;

            _canEditDirection = windowSelectionContext.CanSelectDirection;
            _canEditRing = windowSelectionContext.CanSelectRing;

            UpdatePropertiesOnInit();
        }

        public AvidDirection[] Directions
        {
            get { return _directions; }
        }

        public AvidRingViewModel[] Rings
        {
            get { return _rings; }
        }

        public AvidDirection SelectedDirection
        {
            get { return _window != null ? _window.Direction : AvidDirection.Undefined; }
            set
            {
                if (_window == null)
                {
                    return;
                }

                _window.Direction = value;
                OnPropertyChanged(Properties.SelectedDirection);
            }
        }

        public AvidRingViewModel SelectedRing
        {
            get { return _window != null ? SelectRing(_window.Ring, _window.AbovePlane) : SelectRing(AvidRing.Ember, true); }
            set
            {
                if (_window == null)
                {
                    return;
                }

                _window.Ring = value.Ring;
                _window.AbovePlane = value.IsAbovePlane;

                OnPropertyChanged(Properties.SelectedRing);
                // To maintain integrity we, we set direction to undefined when ring is magenta.
                if (_window.Ring == AvidRing.Magenta)
                {
                    SelectedDirection = AvidDirection.Undefined;
                    _directionLocalLock = true;
                    OnPropertyChanged(Properties.CanEditDirection);
                }
            }
        }

        public string Caption { get; private set; }
        public string Message { get; private set; }

        public bool CanEditDirection
        {
            get { return _window != null && _canEditDirection && !_directionLocalLock; }
        }

        public bool CanEditRing
        {
            get { return _window != null && _canEditRing; }
        }

        public bool IsConfirmed { get; private set; }
        public AvidWindow Result { get; private set; }

        public event Action EditFinished;

        public void Apply()
        {
            IsConfirmed = true;
            Result = _window;

            OnEditFinished();
        }

        private AvidRingViewModel SelectRing(AvidRing ring, bool isAbovePlane)
        {
            return _rings.Single(rn => rn.Ring == ring && rn.IsAbovePlane == isAbovePlane);
        }

        private void UpdatePropertiesOnInit()
        {
            OnPropertyChanged(Properties.Caption);
            OnPropertyChanged(Properties.Message);
            OnPropertyChanged(Properties.SelectedDirection);
            OnPropertyChanged(Properties.SelectedRing);
            OnPropertyChanged(Properties.CanEditDirection);
            OnPropertyChanged(Properties.CanEditRing);
        }

        private void OnEditFinished()
        {
            if (EditFinished != null)
            {
                EditFinished();
            }
        }

        private static class Properties
        {
            public const string Caption = "Caption";
            public const string Message = "Message";
            public const string SelectedDirection = "SelectedDirection";
            public const string SelectedRing = "SelectedRing";
            public const string CanEditDirection = "CanEditDirection";
            public const string CanEditRing = "CanEditRing";
        }
    }
}
