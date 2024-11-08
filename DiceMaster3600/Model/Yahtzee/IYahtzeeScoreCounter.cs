using DiceMaster3600.Core.Enum;
using System;

namespace DiceMaster3600.Model.Yahtzee
{
    public interface IYahtzeeScoreCounter
    {
        void Enable(bool nevim);
        void UpdatePossibleScores(int[] dice);

        event Action<int?, ScoreTypes>? ScoreChanged;
        public event Action<bool>? IsEnabled;
    }
}
