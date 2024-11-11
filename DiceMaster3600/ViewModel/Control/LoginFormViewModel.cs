using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Model.Services;
using DiceMaster3600.View.Dialogs;
using System;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel.Control
{
    public class LoginFormViewModel : NotifyViewModel
    {
        #region Fields
        private readonly IDataAccessManager datamanager;
        #endregion

        #region Properties
        public ICommand SingUpCMD { get; private set; }
        public ICommand RegisterCMD { get; private set; }
        public Action CloseAction { get; set; }
        #endregion

        #region Constructors
        public LoginFormViewModel(IMessageService messageService, IDataAccessManager datamanager, HomeViewModel viewmodel) : base(messageService)
        {
            SingUpCMD = new RelayCommand(() =>
            {
                CloseAction?.Invoke();
                viewmodel.Smazat();

            });

            RegisterCMD = new RelayCommand(() => new EntryForm().ShowDialog());
            SubsribeNotification(NotificationContext.RegistrationSuccess, (m) => Notify(m, MessageType.Success));
            this.datamanager = datamanager;
        }


        public override void Dispose()
        {
            UnsubsribeNotification(NotificationContext.RegistrationSuccess);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Methods
        #endregion

        #region Events
        #endregion
    }
}
