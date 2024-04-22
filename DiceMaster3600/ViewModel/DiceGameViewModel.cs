using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Model.Yahtzee;
using System.Collections.Generic;
using System.Windows.Input;
using System;
using System.Linq;

namespace DiceMaster3600.ViewModel
{
    public class DiceGameViewModel : ObservableObject, IMenuControlViewModel
    {

        #region Fields
        private readonly YahtzeeScoreManager scoreManager;
        #endregion

        #region Properties
        public MenuControlType ControlType => MenuControlType.GamePanel;
        public ICommand RollCommand { get; private set; }
        #endregion

        #region Constructors
        public DiceGameViewModel(){
        }

        public DiceGameViewModel(YahtzeeScoreManager scoreManager)
        {
            this.scoreManager = scoreManager;
            this.RollCommand = new RelayCommand<int[]>((dices) => scoreManager.UpdatePossibleScores(dices!));
        }
        #endregion

        #region Methods
        #endregion

        #region Events
        #endregion
    }
}
