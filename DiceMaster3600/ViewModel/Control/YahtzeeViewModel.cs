using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Model.Yahtzee;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel.Control
{
    public class YahtzeeViewModel
    {
        #region Fields
        #endregion

        #region Properties
        public Dictionary<ScoreTypes, YahtzeeScoreModel> Scores;
        private readonly YahtzeeScoreManager scoreManager;

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
            }
        }
        #endregion

        #region Events
        #endregion
    }
}
