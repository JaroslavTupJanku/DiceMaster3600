using CommunityToolkit.Mvvm.ComponentModel;
using DiceMaster3600.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.ViewModel
{
    public class DiceGameViewModel : ObservableObject, IMenuControlViewModel
    {
        public MenuControlType ControlType => MenuControlType.GamePanel;
    }
}
