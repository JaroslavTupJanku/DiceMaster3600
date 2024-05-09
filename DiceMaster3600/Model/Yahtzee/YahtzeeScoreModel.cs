using CommunityToolkit.Mvvm.ComponentModel;
using DiceMaster3600.Core.Enum;

namespace DiceMaster3600.Model.Yahtzee
{
    //Model pak not ObservableObject
    public class YahtzeeScoreModel : DiceModel
    {
        #region Fields
        #endregion

        #region Properties
        public ScoreTypes ScoreType { get; }
        public bool HasBeenScored { get; private set; }
        #endregion

        #region Constructors
        public YahtzeeScoreModel(ScoreTypes scoreTypes)
        {
            ScoreType = scoreTypes;
            HasBeenScored = false;
        }
        #endregion

        #region Methods

        public void UpdateScore(int score)
        {
            Score = score;
            HasBeenScored = true;
            IsSelected = false;
        }

        #endregion

        #region Events
        #endregion
    }
}
