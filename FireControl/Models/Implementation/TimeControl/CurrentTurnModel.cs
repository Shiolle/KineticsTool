using System;
using FireControl.Models.Interfaces.TimeControl;
using Kinetics.Core.Data;

namespace FireControl.Models.Implementation.TimeControl
{
    internal class CurrentTurnModel : ICurrentTurnModel
    {
        public CurrentTurnModel()
        {
            CurrentTurn = new TurnData();
        }

        public TurnData CurrentTurn { get; private set; }

        public event Action TimeChanged;

        public void AdvanceTime()
        {
            CurrentTurn.AdvanceImpulse();
            OnTimeChanged();
        }

        public void RecedeTime()
        {
            CurrentTurn.RecedeImpulse();
            OnTimeChanged();
        }

        public void SetTime(string newTime)
        {
            CurrentTurn = TurnData.Parse(newTime);
            OnTimeChanged();
        }

        private void OnTimeChanged()
        {
            if (TimeChanged != null)
            {
                TimeChanged();
            }
        }
    }
}
