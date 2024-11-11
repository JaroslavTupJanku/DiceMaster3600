using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace DiceMaster3600.Model.Yahtzee
{
    public class DiceModel : ObservableObject
    {
        private int score = 1;
        private bool isSelected = false;

        public Queue<int> LastThreeResults { get; private set; } = new Queue<int>();
        public int ChangeCounter { get; private set; } = 0;

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


        public void AddResult(int result)
        {
            result = result > 6 ? 6 : result;

            if (LastThreeResults.Count == 3)
            {
                LastThreeResults.Dequeue();
            }
            LastThreeResults.Enqueue(result);

            if (LastThreeResults.Count == 3 && LastThreeResults.All(r => r == result))
            {
                ChangeCounter++;
            }
            else
            {
                ChangeCounter = 0;
            }
        }
    }
}
