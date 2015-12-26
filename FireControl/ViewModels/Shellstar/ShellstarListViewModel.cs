using System;
using System.Collections.Generic;
using FireControl.Infrastructure.Interfaces;
using FireControl.Models.Interfaces;
using FireControl.Models.Interfaces.ShellStars;
using FireControl.Models.Interfaces.UnitControl;

namespace FireControl.ViewModels.Shellstar
{
    internal class ShellstarListViewModel : ViewModelBase, INavigationNode
    {
        private const string CounterFormat = "{0}/{1}";

        private string _counterText;
        private bool _canNavigateLeft;
        private bool _canNavigateRight;
        private int _navigationIndex;
        private int _totalShellstars;

        private IUnitModel _unit;
        private readonly List<IShellstarModel> _attachedShellstars;
        private IShellstarModel _selectedShellstar;

        private readonly ShellstarInfoViewModel _shellstarInfo;

        public ShellstarListViewModel(ShellstarInfoViewModel shellstarInfo)
        {
            _shellstarInfo = shellstarInfo;
            _attachedShellstars = new List<IShellstarModel>();
        }

        public string CounterText 
        {
            get { return _counterText; }
            private set
            {
                _counterText = value;
                OnPropertyChanged(Properties.CounterText);
            }
        }

        public bool CanNavigateLeft
        {
            get { return _canNavigateLeft; }
            set
            {
                _canNavigateLeft = value;
                OnPropertyChanged(Properties.CanNavigateLeft);
            }
        }

        public bool CanNavigateRight
        {
            get { return _canNavigateRight; }
            set
            {
                _canNavigateRight = value;
                OnPropertyChanged(Properties.CanNavigateRight);
            }
        }

        public string Tag
        {
            get { return _selectedShellstar != null ? _selectedShellstar.Tag : string.Empty; }
            set
            {
                if (_selectedShellstar != null)
                {
                    _selectedShellstar.Tag = value;
                    OnPropertyChanged(Properties.Tag);
                }
            }
        }

        public bool IsShellstarSelected
        {
            get { return _unit != null && _selectedShellstar != null; }
        }

        public ShellstarInfoViewModel ShellstarInfo
        {
            get { return _shellstarInfo; }
        }

        public void NavigateLeft()
        {
            Navigate(_navigationIndex - 1);
        }

        public void NavigateRight()
        {
            Navigate(_navigationIndex + 1);
        }

        public void RemoveShellstar()
        {
            if (_unit != null && _selectedShellstar != null)
            {
                _attachedShellstars.Remove(_selectedShellstar);
                _unit.DetachShellstar(_selectedShellstar);
                OnShellstarListChanged();
                NavigateLeft();
            }
        }

        public void Initialize(object dataContext)
        {
            var unit = VerifyContext<IUnitModel>(dataContext);
            if (unit == null)
            {
                throw new ArgumentNullException("dataContext");
            }
            Initialize(unit);
        }

        public void Initialize(IUnitModel unit)
        {
            UnsubscribeFromEvents();
            _unit = unit;
            OnShellstarListChanged();
            Navigate(0);
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            if (_unit == null)
            {
                return;
            }
            _unit.ShellstarListChanged += UnitOnShellstarListChanged;
        }

        private void UnsubscribeFromEvents()
        {
            if (_unit == null)
            {
                return;
            }
            _unit.ShellstarListChanged -= UnitOnShellstarListChanged;
        }

        private void UnitOnShellstarListChanged(ListAction action, IShellstarModel affectedShellstar)
        {
            OnShellstarListChanged();
            Navigate(_navigationIndex);
        }

        private void Navigate(int newIndex)
        {
            _navigationIndex = Trim(newIndex, 0, _totalShellstars - 1);
            UpdateNavigationControls();
            UpdateCounterText();
            _selectedShellstar = _unit != null && _totalShellstars > 0 ? _attachedShellstars[_navigationIndex] : null;

            if (_unit != null)
            {
                _shellstarInfo.Initialize(_selectedShellstar, _unit.Name);
            }

            OnPropertyChanged(Properties.Tag);
            OnPropertyChanged(Properties.IsShellstarSelected);
        }

        private void UpdateNavigationControls()
        {
            CanNavigateLeft = _navigationIndex > 0;
            CanNavigateRight = _navigationIndex < _totalShellstars - 1;
        }

        private void UpdateCounterText()
        {
            if (_unit != null)
            {
                if (_totalShellstars > 0)
                {
                    CounterText = string.Format(CounterFormat, _navigationIndex + 1, _totalShellstars);
                    return;
                }
                CounterText = string.Format(CounterFormat, 0, _totalShellstars);
                return;
            }
            CounterText = string.Empty;
        }

        private void OnShellstarListChanged()
        {
            _totalShellstars = _unit != null ? _unit.AttachedShellstars.Count : 0;
            _attachedShellstars.Clear();
            if (_unit != null)
            {
                _attachedShellstars.AddRange(_unit.AttachedShellstars);
            }
        }

        private static int Trim(int currentValue, int minValue, int maxValue, int fallbackValue = 0)
        {
            if (minValue > maxValue)
            {
                return fallbackValue;
            }

            if (currentValue < minValue)
            {
                return minValue;
            }

            if (currentValue > maxValue)
            {
                return maxValue;
            }

            return currentValue;
        }

        private static class Properties
        {
            public const string Tag = "Tag";
            public const string CounterText = "CounterText";
            public const string CanNavigateLeft = "CanNavigateLeft";
            public const string CanNavigateRight = "CanNavigateRight";
            public const string IsShellstarSelected = "IsShellstarSelected";
        }
    }
}
