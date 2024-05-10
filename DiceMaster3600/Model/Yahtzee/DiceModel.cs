using CommunityToolkit.Mvvm.ComponentModel;

namespace DiceMaster3600.Model.Yahtzee
{
    public class DiceModel : ObservableObject
    {
        private int score = 1;
        private bool isSelected = false;

        public int Score
        {
            get { return score; }
            set { SetProperty(ref score, value); }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }
    }
}
