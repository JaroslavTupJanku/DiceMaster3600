using CommunityToolkit.Mvvm.ComponentModel;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Model.Services;
using MaterialDesignThemes.Wpf;
using System;

namespace DiceMaster3600.ViewModel
{
    public abstract class NotifyViewModel : ObservableObject, IDisposable
    {
        #region Fields
        private readonly IMessageService messageService;
        private MessageType messagetype;
        #endregion

        #region Properties
        public enum MessageType
        {
            Failed,
            Success,
            Warning
        }

        public MessageType CurrentMessageType
        {
            get => messagetype;
            set => SetProperty(ref messagetype, value);
        }

        public SnackbarMessageQueue? MessageQueue => messageService?.MessageQueue;
        #endregion

        #region Constructors
        public NotifyViewModel(IMessageService messageService)
        {
            this.messageService = messageService;
        }
        #endregion

        #region Methods

        protected void Notify(NotificationContext c, string mess) => messageService.TryNotify(c, mess);
        protected void SubsribeNotification(NotificationContext c, Action<string> callback) => messageService.Subscribe(c, callback);
        protected void UnsubsribeNotification(NotificationContext c) => messageService.Unsubscribe(c);

        protected virtual void EnqueueMessage(string message, MessageType type)
        {
            CurrentMessageType = type;
            messageService.EnqueueMessage(message);
        }

        public abstract void Dispose();
        #endregion

        #region Events
        #endregion
    }
}
