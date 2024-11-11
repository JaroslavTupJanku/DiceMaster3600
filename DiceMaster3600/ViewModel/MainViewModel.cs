using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Model;
using DiceMaster3600.Model.Services;
using DiceMaster3600.View;
using DiceMaster3600.View.Dialogs;
using System;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel
{
    public class MainViewModel : NotifyViewModel
    {
        #region Fields
        private bool isPlayerLogged;
        private object? currentView;

        private readonly IActiveUserManager activeUser;
        private readonly IViewModelFactory factory;
        #endregion

        #region Properties
        public bool IsPlayerLogged
        {
            get => isPlayerLogged;
            private set => SetProperty(ref isPlayerLogged, value);
        }

        public object? CurrentView
        {
            get => currentView;
            private set => SetProperty(ref currentView, value);
        }

        public ICommand LogCommand { get; private set; }
        public IRelayCommand<MenuControlType> MenuCommad { get; private set; }

        #endregion

        #region Constructors
        public MainViewModel(IActiveUserManager activeUser, IViewModelFactory factory, 
                             IMessageService messageService, IProcessManager provider) : base(messageService)
        {
            this.activeUser = activeUser;
            this.factory = factory;
            provider.RegisterAllProcesses();

            CurrentView = factory.CreateViewModel<DiceGameViewModel>() 
                ?? throw new InvalidOperationException("HomeViewModel cannot be created.");

            MenuCommad = new RelayCommand<MenuControlType>((type) => OnChangeView(type));
            LogCommand = new RelayCommand(LogIn);
        }
        #endregion

        #region Methods
        private void LogIn()
        {
            IsPlayerLogged = !IsPlayerLogged;

            _ = IsPlayerLogged ? new EntryForm().ShowDialog() : activeUser.Logout();
        }

        private void OnChangeView(MenuControlType viewName)
        {
            try
            {
                CurrentView = viewName switch
                {
                    MenuControlType.DashBoard => factory.CreateViewModel<HomeViewModel>(),
                    MenuControlType.GamePanel => factory.CreateViewModel<DiceGameViewModel>(),
                    MenuControlType.AdminPanel => throw new NotImplementedException(),
                    MenuControlType.About => throw new NotImplementedException(),
                    _ => throw new NotImplementedException()
                };
            }
            catch (Exception ex)
            {
                Notify(ex.Message, MessageType.Failed);
            }
        }

        public override void Dispose() => GC.SuppressFinalize(this);
        #endregion

        #region Events
        #endregion
    }
}
