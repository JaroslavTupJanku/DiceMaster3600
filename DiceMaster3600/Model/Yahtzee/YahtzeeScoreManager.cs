﻿using DiceMaster3600.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceMaster3600.Model.Yahtzee
{
    public class YahtzeeScoreManager
    {
        #region Fields
        readonly Random random = new();
        public int[] currentDiceValues = new int[5];
        private Dictionary<ScoreTypes, Func<int[], int>>? scoreCalculators;
        #endregion

        #region Properties
        public Dictionary<ScoreTypes, YahtzeeScoreModel>? Scores { get; private set; }
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
            Scores = new Dictionary<ScoreTypes, YahtzeeScoreModel>();
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

            foreach (ScoreTypes scoreType in Enum.GetValues(typeof(ScoreTypes)))
            {
                Scores.Add(scoreType, new YahtzeeScoreModel(scoreType, false, 0));
            }
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
            foreach (var scoreType in Scores!.Keys)
            {
                if (!Scores[scoreType].HasBeenScored)
                {
                    var possibleScore = scoreCalculators![scoreType](currentDiceValues);
                    Scores[scoreType].Enable(possibleScore > 0);
                }
            }
        }

        public void AssignScoreToCategory(ScoreTypes selectedType)
        {
            if (Scores![selectedType].IsSelected && !Scores[selectedType].HasBeenScored)
            {
                var scoreValue = scoreCalculators![selectedType](currentDiceValues);
                Scores[selectedType].UpdateScore(scoreValue);
                ScoreChanged?.Invoke();
            }
        }

        public int GetCurrentTotalScore() => Scores!.Values.Where(s => s.HasBeenScored).Sum(s => s.Score);
        public void GenerateDiceRolls() => UpdatePossibleScores(Enumerable.Range(0, 5).Select(x => random.Next(1, 7)).ToArray());
        #endregion

        #region Events
        public event Action? ScoreChanged;
        #endregion

    }
}
