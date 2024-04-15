using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Model;
using DiceMaster3600.View;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        #region Fields
        private bool isPlayerLogged;
        private readonly IActiveUserManager activeUser;
        private readonly IViewModelFactory factory;
        #endregion

        #region Properties
        public bool IsPlayerLogged
        {
            get => isPlayerLogged;
            private set => SetProperty(ref isPlayerLogged, value);
        }

        public object? CurrentView { get; private set; }

        public ICommand LogCommand { get; private set; }
        public IRelayCommand<MenuControlType> MenuCommad { get; private set; }

        #endregion

        #region Constructors
        public MainViewModel(IActiveUserManager activeUser, IViewModelFactory factory)
        {
            this.activeUser = activeUser;
            this.factory = factory;

            CurrentView = factory.CreateViewModel<HomeViewModel>();
            MenuCommad = new RelayCommand<MenuControlType>((type) => OnChangeView(type));
            LogCommand = new RelayCommand(LogIn);

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

        private void OnChangeView(MenuControlType viewName)
        {
            switch (viewName)
            {
                case MenuControlType.DashBoard: CurrentView = factory.CreateViewModel<HomeViewModel>(); break;
                case MenuControlType.GamePanel: CurrentView = factory.CreateViewModel<DiceGameViewModel>(); break;
            }
        }

        #endregion

        #region Events
        #endregion
    }
}
