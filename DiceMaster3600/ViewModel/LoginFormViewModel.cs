using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.View.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel
{
    public class LoginFormViewModel : ObservableObject
    {
        #region Fields
        #endregion

        #region Properties
        public ICommand SingUpCMD { get; private set; }
        #endregion

        #region Constructors
        public LoginFormViewModel()
        {
            SingUpCMD = new RelayCommand(() => new EntryForm().ShowDialog());
        }
        #endregion

        #region Methods
        #endregion

        #region Events
        #endregion
    }
}
