using DiceMaster3600.Core.Enum;
using DiceMaster3600.Model.Services;

namespace DiceMaster3600.ViewModel
{
    public class AdminPanelViewModel : NotifyViewModel, IMenuControlViewModel
    {
        #region Fields
        private bool isPlayerLogged;
        #endregion

        #region Properties

        public MenuControlType ControlType => MenuControlType.AdminPanel;

        public bool IsPlayerLogged
        {
            get => isPlayerLogged;
            private set => SetProperty(ref isPlayerLogged, value);
        }
        #endregion

        #region Constructors
        public AdminPanelViewModel(IMessageService messageService) : base(messageService)
        {
        }
        #endregion

        #region Methods
        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }

        private void LogIn()
        {
            IsPlayerLogged = !IsPlayerLogged;
            //_ = IsPlayerLogged ? new EntryForm().ShowDialog() : activeUser.Logout();
        }
        #endregion

        #region Events
        #endregion
    }
}
