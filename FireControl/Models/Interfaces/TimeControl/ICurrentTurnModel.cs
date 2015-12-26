using System;
using Kinetics.Core.Data;

namespace FireControl.Models.Interfaces.TimeControl
{
    /// <summary>
    /// Stores info about current turn and segment and allows to change it.
    /// </summary>
    internal interface ICurrentTurnModel
    {
        TurnData CurrentTurn { get; }

        event Action TimeChanged;

        void AdvanceTime();

        void RecedeTime();

        void SetTime(string newTime);
    }
}
