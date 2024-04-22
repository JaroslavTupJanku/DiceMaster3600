using CommunityToolkit.Mvvm.ComponentModel;
using DiceMaster3600.Core.Enum;

namespace DiceMaster3600.Model.Yahtzee
{
    public class YahtzeeScoreModel : ObservableObject
    {

        #region Fields
        private bool isEnabled;
        private int? score;
        #endregion

        #region Properties
        public ScoreTypes ScoreType { get; }
        public bool HasBeenScored { get; private set; }
        public bool IsEnabled
        {
            get => isEnabled;
            private set => SetProperty(ref isEnabled, value);
        }

        public int? Score
        {
            get => score;
            private set => SetProperty(ref score, value);
        }
        #endregion

        #region Constructors
        public YahtzeeScoreModel(ScoreTypes scoreTypes, bool isEnabled, int? score)
        {
            ScoreType = scoreTypes;
            IsEnabled = isEnabled;
            Score = score;
            HasBeenScored = false;
        }
        #endregion

        #region Methods
        public void Enable(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        public void UpdateScore(int score)
        {
            Score = score;
            HasBeenScored = true;
            IsEnabled = false;
        }

        #endregion

        #region Events
        #endregion
    }
}
