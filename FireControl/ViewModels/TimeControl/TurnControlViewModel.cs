using FireControl.Models.Interfaces.TimeControl;

namespace FireControl.ViewModels.TimeControl
{
    internal class TurnControlViewModel : ViewModelBase
    {
        private readonly ICurrentTurnModel _currentTurnModel;

        public TurnControlViewModel(ICurrentTurnModel currentTurnModel)
        {
            _currentTurnModel = currentTurnModel;
            _currentTurnModel.TimeChanged += _currentTurnModel_TimeChanged;
        }

        void _currentTurnModel_TimeChanged()
        {
            OnPropertyChanged(Properties.TurnInfo);
        }

        public string TurnInfo
        {
            get
            {
                return _currentTurnModel.CurrentTurn.ToString();
            }
            set
            {
                _currentTurnModel.SetTime(value);
                OnPropertyChanged(Properties.TurnInfo);
            }
        }

        public void AdvanceTime()
        {
            _currentTurnModel.AdvanceTime();
        }

        public void RecedeTime()
        {
            _currentTurnModel.RecedeTime();
        }

        private static class Properties
        {
            public const string TurnInfo = "TurnInfo";
        }
    }
}
