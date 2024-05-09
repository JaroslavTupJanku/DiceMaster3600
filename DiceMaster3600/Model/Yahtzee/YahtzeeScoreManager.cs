using DiceMaster3600.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceMaster3600.Model.Yahtzee
{
    public class YahtzeeScoreManager : IYahtzeeScoreManager
    {
        #region Fields
        readonly Random random = new();
        public int[] currentDiceValues = new int[5];
        private Dictionary<ScoreTypes, Func<int[], int>>? scoreCalculators;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public YahtzeeScoreManager()
        {
            Initiate();
        }
        #endregion

        #region Methods
        private void Initiate()
        {
            scoreCalculators = new Dictionary<ScoreTypes, Func<int[], int>>()
            {
                { ScoreTypes.Aces, dice => dice.Count(n => n == 1) * 1 },
                { ScoreTypes.Twos, dice => dice.Count(n => n == 2) * 2 },
                { ScoreTypes.Threes, dice => dice.Count(n => n == 3) * 3 },
                { ScoreTypes.Fours, dice => dice.Count(n => n == 4) * 4 },
                { ScoreTypes.Fives, dice => dice.Count(n => n == 5) * 5 },
                { ScoreTypes.Sixes, dice => dice.Count(n => n == 6) * 6 },
                { ScoreTypes.ThreeOfAKind, dice => dice.GroupBy(n => n).Any(g => g.Count() >= 3) ? dice.Sum() : 0 },
                { ScoreTypes.FourOfAKind, dice => dice.GroupBy(n => n).Any(g => g.Count() >= 4) ? dice.Sum() : 0 },
                { ScoreTypes.SmallStraight, dice => HasStraight(dice, 4) ? 30 : 0 },
                { ScoreTypes.LargeStraight, dice => HasStraight(dice, 5) ? 40 : 0 },
                { ScoreTypes.Chance, dice => dice.Sum() },
                { ScoreTypes.HighestScore, dice => dice.GroupBy(n => n).Any(g => g.Count() == 5) ? 50 : 0 },
                { ScoreTypes.FullHouse, FullHouse }
            };
        }

        private int FullHouse(int[] dice)
        {
            var groups = dice.GroupBy(n => n).ToList();
            bool hasThreeOfAKind = groups.Any(g => g.Count() == 3);
            bool hasPair = groups.Any(g => g.Count() == 2);
            return (hasThreeOfAKind && hasPair) ? 25 : 0;
        }

        private bool HasStraight(int[] dice, int straightLength)
        {
            var distinctSortedDice = dice.Distinct().OrderBy(n => n).ToArray();
            int consecutiveNumbers = 1;

            for (int i = 1; i < distinctSortedDice.Length; i++)
            {
                consecutiveNumbers = (distinctSortedDice[i] == distinctSortedDice[i - 1] + 1) ? consecutiveNumbers + 1 : 1;
                if (consecutiveNumbers >= straightLength)
                    return true;
            }

            return false;
        }

        public void UpdatePossibleScores(int[] dice)
        {
            currentDiceValues = dice;
            foreach (var calculator in scoreCalculators!)
            {
                var possibleScore = calculator.Value?.Invoke(dice);
                ScoreChanged?.Invoke((possibleScore, calculator.Key));
            }
        }

        //public void AssignScoreToCategory(ScoreTypes selectedType)
        //{
        //    var model = YahtzeeModels?.FirstOrDefault(x=> x.ScoreType == selectedType);
        //    if ( model != null && model.IsSelected && !model.HasBeenScored)
        //    {
        //        var scoreValue = scoreCalculators![selectedType](currentDiceValues);
        //        model.UpdateScore(scoreValue);
        //        ScoreChanged?.Invoke();
        //    }
        //}

        //public int GetUpperSum() => YahtzeeModels!.Where(model => model.ScoreType >= ScoreTypes.Aces && model.ScoreType <= ScoreTypes.Sixes).Sum(x => x.Score);
        //public int GetLowerSum() => YahtzeeModels!.Where(model => model.ScoreType < ScoreTypes.Aces || model.ScoreType > ScoreTypes.Sixes).Sum(x => x.Score);
       // public int GetCurrentTotalScore() => YahtzeeModels!.Where(s => s.HasBeenScored).Sum(s => s.Score);
        public void GenerateDiceRolls() => UpdatePossibleScores(Enumerable.Range(0, 5).Select(x => random.Next(1, 7)).ToArray());
        #endregion

        #region Events
        public event Action<(int?, ScoreTypes)>? ScoreChanged;
        #endregion

    }
}
