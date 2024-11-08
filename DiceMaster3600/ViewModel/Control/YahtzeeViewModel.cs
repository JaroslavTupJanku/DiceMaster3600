using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Model.Yahtzee;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel.Control
{
    public class YahtzeeViewModel : ObservableObject
    {
        #region Fields
        private int upperSectionScore;
        private int lowerSectionScore;
        private int extraBonus;

        private int totalScoreUpperSection;
        private int totalScore;
        private bool isEnabled;
        #endregion

        #region Properties
        public int UpperSectionScore
        {
            get => upperSectionScore;
            set
            {
                SetProperty(ref upperSectionScore, value);
                if (value >= 63)
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

        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
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

        public ObservableCollection<YahtzeeScoreModel> YahtzeeModels { get; } = new();
        public ICommand ScoreCommand { get; private set; }
        #endregion

        #region Constructors
        public YahtzeeViewModel(IYahtzeeScoreCounter scoreManager)
        {
            InitiateCollection();
            IsEnabled = true;
            scoreManager.ScoreChanged += ScoreManager_ScoreChanged1;
            ScoreCommand = new RelayCommand<ScoreTypes>((st) => SelectCategory(st));
            scoreManager.IsEnabled += (st) => IsEnabled = st;
        }

        private void ScoreManager_ScoreChanged1(int? score, ScoreTypes type)
        {
            var model = YahtzeeModels.FirstOrDefault(x => x.ScoreType == type);
            if (model != null && !model.HasBeenScored && score.HasValue)
            {
                model.Score = score.Value;
                model.IsSelected = true;
            }
        }
        #endregion

        #region Methods
        private void InitiateCollection()
        {
            foreach (ScoreTypes scoreType in Enum.GetValues(typeof(ScoreTypes)))
            {
                YahtzeeModels?.Add(new YahtzeeScoreModel(scoreType));
            }
        }

        private void SelectCategory(ScoreTypes st)
        {
            var model = YahtzeeModels.FirstOrDefault((x) => x.ScoreType == st);
            if (model != null && !model.HasBeenScored)
            {
                model.HasBeenScored = true;
                UpperSectionScore = GetUpperSum();
                LowerSectionScore = GetLowerSum();
                TotalScore = UpperSectionScore + LowerSectionScore;
                IsEnabled = false;
            }
        }


        public int GetUpperSum() => YahtzeeModels!.Where(x => x.ScoreType >= ScoreTypes.Aces && x.ScoreType <= ScoreTypes.Sixes && x.HasBeenScored)
                                                  .Sum(x => x.Score);
        public int GetLowerSum() => YahtzeeModels!.Where(x => x.ScoreType < ScoreTypes.Aces || x.ScoreType > ScoreTypes.Sixes && x.HasBeenScored)
                                                  .Sum(x => x.Score);
        public int GetCurrentTotalScore() => YahtzeeModels!.Where(s => s.HasBeenScored).Sum(s => s.Score);
        #endregion

        #region Events
        #endregion
    }
}
