using DiceMaster3600.Core.Enum;

namespace DiceMaster3600.Model.Yahtzee
{
    public class YahtzeeScoreModel : DiceModel
    {
        #region Fields
        private bool hasBeenScored;
        #endregion

        #region Properties
        public ScoreTypes ScoreType { get; }

        public bool HasBeenScored
        {
            get { return hasBeenScored; }
            set { SetProperty(ref hasBeenScored, value); }
        }
        #endregion

        #region Constructors
        public YahtzeeScoreModel(ScoreTypes scoreTypes)
        {
            ScoreType = scoreTypes;
            HasBeenScored = false;
            Score = 0;
        }
        #endregion

        #region Methods
        #endregion

        #region Events
        #endregion
    }
}
