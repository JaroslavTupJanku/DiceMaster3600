using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Model.Yahtzee;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel.Control
{
    public class YahtzeeViewModel : ObservableObject
    {
        #region Fields
        private readonly YahtzeeScoreManager scoreManager;

        private int upperSectionScore;
        private int lowerSectionScore;
        private int extraBonus;

        private int totalScoreUpperSection;
        private int totalScore;
        #endregion

        #region Properties
        public int UpperSectionScore
        {
            get => upperSectionScore;
            set
            {
                SetProperty(ref upperSectionScore, value);
                if (value <= 35)
                {
                    ExtraBonus = 35;
                    TotalScoreUpperSection = value + 35;
                }
            }              
        }

        public int LowerSectionScore
        {
            get => lowerSectionScore;
            set => SetProperty(ref lowerSectionScore, value);
        }

        public int TotalScore
        {
            get => totalScore;
            set => SetProperty(ref totalScore, value);
        }

        public int TotalScoreUpperSection
        {
            get => totalScoreUpperSection;
            set => SetProperty(ref totalScoreUpperSection, value);
        }

        public int ExtraBonus
        {
            get => extraBonus;
            set => SetProperty(ref extraBonus, value);
        }

        public Dictionary<ScoreTypes, YahtzeeScoreModel> Scores;
        public ICommand SelectCategoryCommand { get; private set; }
        #endregion

        #region Constructors
        public YahtzeeViewModel(YahtzeeScoreManager scoreManager)
        {
            this.scoreManager = scoreManager;

            Scores = scoreManager?.Scores ?? throw new Exception("fdas");
            SelectCategoryCommand = new RelayCommand<ScoreTypes>((st) => SelectCategory(st));
        }
        #endregion

        #region Methods

        private void SelectCategory(ScoreTypes st)
        {
            if (st is ScoreTypes scoreType)
            {
                scoreManager.AssignScoreToCategory(scoreType);
                UpperSectionScore = scoreManager.GetUpperSum();
                LowerSectionScore = scoreManager.GetLowerSum();
                TotalScore = UpperSectionScore + LowerSectionScore;
            }
        }
        #endregion

        #region Events
        #endregion
    }
}
