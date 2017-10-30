using System;
using System.Linq;
using Kinetics.Core.Data.FiringSolution;
using Kinetics.Core.Data.HexGrid;

namespace FireControl.ViewModels.Counterfire
{
    internal class CounterfireImpulseViewModel : ViewModelBase
    {
        private int _counterfireRange;

        public CounterfireImpulseViewModel(ImpulseTrackElement impulseTrackElement, HexGridCoordinate[] positions, int counterfireRange)
        {
            if (impulseTrackElement == null || positions == null || positions.Length == 0)
            {
                throw new ArgumentException("Cannot initialize counterfire impulse without impulse data and posiions.");
            }

            Impulse = impulseTrackElement.Impulse.ToString();
            Range = impulseTrackElement.Range;
            Positions = positions.Select(ps => ps.ToString()).ToArray();
            UpdateCounterfireRange(counterfireRange);
        }

        public string Impulse { get; private set; }

        public int Range { get; private set; }

        public string[] Positions { get; private set; }

        public string CounterfireRange { get; private set; }

        private void UpdateCounterfireRange(int newRange)
        {
            if (_counterfireRange == newRange)
            {
                return;
            }

            _counterfireRange = newRange;
            CounterfireRange = newRange != 0 ? newRange.ToString("D") : string.Empty;
            OnPropertyChanged(Properties.CounterfireRange);
        }

        private static class Properties
        {
            public const string CounterfireRange = "CounterfireRange";
        }
    }
}
