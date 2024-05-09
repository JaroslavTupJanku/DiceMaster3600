using DiceMaster3600.Core.Enum;
using System.Collections.Generic;

namespace DiceMaster3600.Model.Yahtzee
{
    public interface IYahtzeeScoreManager
    {
        void AssignScoreToCategory(ScoreTypes scoreType);
        int GetUpperSum();
        int GetLowerSum();
        void UpdatePossibleScores(int[] dice);
    }
}
