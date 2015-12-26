using System;
using System.Collections.Generic;
using System.Linq;
using FireControl.Infrastructure.Interfaces;
using FireControl.Models.Interfaces.ShellStars;
using FireControl.ViewModels.DataContexts;
using Kinetics.Core.Data.Avid;

namespace FireControl.ViewModels.Shellstar
{
    internal class LaunchWindowControlViewModel : ViewModelBase
    {
        private LaunchWindowViewModel[] _availableWindows;
        private LaunchWindowViewModel _selectedWindow;

        private readonly INavigationInterface _selectReferenceDirection;
        private readonly INavigationInterface _selectLaunchWindow;

        private IEvasionInfoModel _evasionInfo;

        public LaunchWindowControlViewModel(INavigationService navigationService)
        {
            _selectReferenceDirection = navigationService.GetAvidWindowSelectionRequest(AcquireReferenceDirectionContext, ProcessReferenceDirectionSelection);
            _selectLaunchWindow = navigationService.GetAvidWindowSelectionRequest(AcquireLaunchWindowContext, ProcessLaunchWindowSelection);
        }

        public void Initialize(IEvasionInfoModel evasionInfo, AvidWindow[] availableWindows)
        {
            _evasionInfo = evasionInfo;
            PrepareWindowList(availableWindows);
        }

        public LaunchWindowViewModel SelectedWindow
        {
            get { return _selectedWindow; }
            set
            {
                var newValue = value;
                _selectedWindow = newValue;
                if (newValue == null)
                {
                    _evasionInfo.UpdateDirections(null, AvidDirection.Undefined);
                    OnPropertyChanged(Properties.SelectedWindow);
                    OnPropertyChanged(Properties.ReferenceDirectionAvailable);
                    SelectLaunchWindow.Navigate();
                    return;
                }

                _evasionInfo.UpdateDirections(newValue.Window, newValue.ReferenceDirection);
                OnPropertyChanged(Properties.SelectedWindow);
                OnPropertyChanged(Properties.ReferenceDirectionAvailable);
                if (newValue.NeedToSetReferenceDirection)
                {
                    SelectReferenceDirection.Navigate();
                }
            }
        }

        public LaunchWindowViewModel[] AvailableWindows
        {
            get { return _availableWindows; }
        }

        public bool ReferenceDirectionAvailable
        {
            get { return SelectedWindow != null && SelectedWindow.CanSetReferenceDirection; }
        }

        public bool IsDataAvailable
        {
            get { return SelectedWindow != null; }
        }

        public INavigationInterface SelectReferenceDirection
        {
            get { return _selectReferenceDirection; }
        }

        public INavigationInterface SelectLaunchWindow
        {
            get { return _selectLaunchWindow; }
        }

        public void SetDefaultWindow()
        {
            var selection = _availableWindows.FirstOrDefault(wnd => !wnd.NeedToSetReferenceDirection);
            if (selection == null)
            {
                selection = _availableWindows.FirstOrDefault();
            }
            SelectedWindow = selection;
        }

        private object AcquireLaunchWindowContext()
        {
            var initialWinow = SelectedWindow != null ? SelectedWindow.Window : new AvidWindow(AvidDirection.A, AvidRing.Ember, true);
            return new WindowSelectionContext
            {
                CanSelectDirection = true,
                CanSelectRing = true,
                InitialWindow = initialWinow,
                Caption = "Select launch window.",
                Message = "This result will override all automatically selected launch windows. You will NOT be able to revert this operation."
            };
        }

        private void ProcessLaunchWindowSelection(AvidWindow result)
        {
            if (result != null)
            {
                // Overwriting all previous launch windows.
                PrepareWindowList(new[] { result });
            }
        }

        private object AcquireReferenceDirectionContext()
        {
            if (_selectedWindow == null)
            {
                throw new NullReferenceException("Direction selection was called while the window is not yet selected.");
            }

            string comment = string.Format("Please select reference direction for the launch window {0}.{1}Per F1.253, this direction should be the same as the direction of the target's top marker, or nose marker if the former is in the magenta ring.", _selectedWindow.Name, Environment.NewLine);

            var window = new AvidWindow(_selectedWindow.ReferenceDirection != AvidDirection.Undefined ? _selectedWindow.ReferenceDirection : AvidDirection.A, AvidRing.Ember, true);

            return new WindowSelectionContext
            {
                CanSelectDirection = true,
                CanSelectRing = false,
                InitialWindow = window,
                Caption = "Select reference direction",
                Message = comment
            };
        }

        private void ProcessReferenceDirectionSelection(AvidWindow result)
        {
            _evasionInfo.UpdateDirections(SelectedWindow.Window, result.Direction);
            _selectedWindow.ReferenceDirection = result.Direction;
        }

        private void PrepareWindowList(IEnumerable<AvidWindow> launchWindows)
        {
            _availableWindows = _evasionInfo != null ?
                launchWindows.Select(wnd => new LaunchWindowViewModel(wnd)).ToArray() :
                new LaunchWindowViewModel[0];

            OnPropertyChanged(Properties.AvailableWindows);
            SetDefaultWindow();
        }

        private static class Properties
        {
            public const string AvailableWindows = "AvailableWindows";
            public const string SelectedWindow = "SelectedDirection";
            public const string ReferenceDirectionAvailable = "ReferenceDirectionAvailable";
        }
    }
}
