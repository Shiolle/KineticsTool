using System;

namespace Kinetics.Core.Data
{
    public class TurnData : IEquatable<TurnData>
    {
        private const char ImpulseSeparator = '.';

        public TurnData()
            : this (1, 1) { }

        public TurnData(int turn, int impulse)
        {
            if (turn < 1)
            {
                throw new ArgumentException(string.Format("Turn number should be positive, but is actually {0}", turn), "turn");
            }

            if (impulse < 1 || impulse > 8)
            {
                throw new ArgumentException(string.Format("Impulse should be between 1 and 8 but is actually {0}", impulse), "impulse");
            }

            Turn = turn;
            Impulse = impulse;
        }

        public int Impulse { get; private set; }
        public int Turn { get; private set; }

        public void AdvanceImpulse()
        {
            Impulse++;
            if (Impulse > 8)
            {
                Impulse = 1;
                Turn++;
            }
        }

        public void RecedeImpulse()
        {
            if (Impulse <= 1 && Turn <= 1)
            {
                return;
            }

            Impulse--;
            if (Impulse < 1)
            {
                Turn--;
                Impulse = 8;
            }
        }

        public TurnData DeepCopy()
        {
            return new TurnData(Turn, Impulse);
        }

        public TurnData GetNextImpulse()
        {
            var copy = DeepCopy();
            copy.AdvanceImpulse();
            return copy;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}", Turn, ImpulseSeparator, Impulse);
        }

        public static TurnData Parse(string source)
        {
            string[] turnDataComponents = source.Split(ImpulseSeparator);

            if (turnDataComponents == null || turnDataComponents.Length != 2)
            {
                throw new ArgumentException(string.Format("Cannot convert turn data. Incorrect format: {0}.", source), "source");
            }

            return new TurnData(Convert.ToInt32(turnDataComponents[0]), Convert.ToInt32(turnDataComponents[1]));
        }

        #region Equality logic

        public bool Equals(TurnData other)
        {
            return Turn == other.Turn && Impulse == other.Impulse;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TurnData;

            if (other != null)
            {
                return Equals(other);
            }

            var otherStr = obj as string;

            if (otherStr != null)
            {
                return this.ToString().Equals(otherStr, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (Turn - 1) * 8 + Impulse - 1;
        }

        #endregion
    }
}