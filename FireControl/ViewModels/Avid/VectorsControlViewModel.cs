using System;
using FireControl.Models.Interfaces.UnitControl;
using Kinetics.Core.Data.HexVectors;

namespace FireControl.ViewModels.Avid
{
    internal class VectorsControlViewModel : ViewModelBase
    {
        private IUnitModel _selectedUnit;

        public int ComponentA
        {
            get { return GetComponent(HexAxis.A); }
            set
            {
                AssingComponentValue(value, HexAxis.A);
                OnPropertyChanged(Properties.ComponentA);
            }
        }

        public int ComponentB
        {
            get { return GetComponent(HexAxis.B); }
            set
            {
                AssingComponentValue(value, HexAxis.B);
                OnPropertyChanged(Properties.ComponentB);
            }
        }

        public int ComponentC
        {
            get { return GetComponent(HexAxis.C); }
            set
            {
                AssingComponentValue(value, HexAxis.C);
                OnPropertyChanged(Properties.ComponentC);
            }
        }

        public int ComponentD
        {
            get { return GetComponent(HexAxis.D); }
            set
            {
                AssingComponentValue(value, HexAxis.D);
                OnPropertyChanged(Properties.ComponentD);
            }
        }

        public int ComponentE
        {
            get { return GetComponent(HexAxis.E); }
            set
            {
                AssingComponentValue(value, HexAxis.E);
                OnPropertyChanged(Properties.ComponentE);
            }
        }

        public int ComponentF
        {
            get { return GetComponent(HexAxis.F); }
            set
            {
                AssingComponentValue(value, HexAxis.F);
                OnPropertyChanged(Properties.ComponentF);
            }
        }

        public int ComponentDown
        {
            get { return GetComponent(HexAxis.Down); }
            set
            {
                AssingComponentValue(value, HexAxis.Down);
                OnPropertyChanged(Properties.ComponentDown);
            }
        }

        public int ComponentUp
        {
            get { return GetComponent(HexAxis.Up); }
            set
            {
                AssingComponentValue(value, HexAxis.Up);
                OnPropertyChanged(Properties.ComponentUp);
            }
        }

        public void SelectUnit(IUnitModel unit)
        {
            _selectedUnit = unit;
            OnNewUnitSelected();
        }

        private void OnNewUnitSelected()
        {
            OnPropertyChanged(Properties.ComponentA);
            OnPropertyChanged(Properties.ComponentB);
            OnPropertyChanged(Properties.ComponentC);
            OnPropertyChanged(Properties.ComponentD);
            OnPropertyChanged(Properties.ComponentE);
            OnPropertyChanged(Properties.ComponentF);
            OnPropertyChanged(Properties.ComponentUp);
            OnPropertyChanged(Properties.ComponentDown);
        }

        private int GetComponent(HexAxis axis)
        {
            if (_selectedUnit == null || _selectedUnit.Vectors == null)
            {
                return 0;
            }

            return _selectedUnit.GetDirectionValue(axis);
        }

        private void AssingComponentValue(int value, HexAxis axis)
        {
            if (_selectedUnit == null || _selectedUnit.Vectors == null)
            {
                return;
            }

            _selectedUnit.AssginDirectionValue(axis, (uint)Math.Abs(value));
        }

        private static class Properties
        {
            public const string ComponentA = "ComponentA";
            public const string ComponentB = "ComponentB";
            public const string ComponentC = "ComponentC";
            public const string ComponentD = "ComponentD";
            public const string ComponentE = "ComponentE";
            public const string ComponentF = "ComponentF";
            public const string ComponentUp = "ComponentUp";
            public const string ComponentDown = "ComponentDown";
        }
    }
}
