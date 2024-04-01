using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Model;
using DiceMaster3600.View;
using System;
using System.Windows.Automation;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        #region Fields
        private bool isPlayerLogged;
        private readonly IActiveUserManager activeUser;
        #endregion

        #region Properties
        public HomeViewModel HomeVM { get; private set; }
        public object CurrentView { get; private set; }
        public ICommand LogCommand { get; private set; }
        public bool IsPlayerLogged
        {
            get => isPlayerLogged;
            private set => SetProperty(ref isPlayerLogged, value);
        }
        #endregion

        #region Constructors
        public MainViewModel(IDataAccessManager manager, IActiveUserManager activeUser)
        {
            this.activeUser = activeUser;

            HomeVM = new HomeViewModel(manager);
            CurrentView = HomeVM;

            LogCommand = new RelayCommand(() => LogIn());

        }
        #endregion

        #region Methods
        private void LogIn()
        {
            IsPlayerLogged = !IsPlayerLogged;

            _ = IsPlayerLogged
                ? new LoginForm().ShowDialog()
                : activeUser.Logout();
        }

        #endregion

        #region Events
        #endregion









    }
}
