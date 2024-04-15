using DiceMaster3600.Core.Enum;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;

namespace DiceMaster3600.Model.Services
{
    public class SnackbarMessageService : IMessageService
    {
        #region Fields
        private readonly SnackbarMessageQueue messageQueue;
        private readonly Dictionary<NotificationContext, Action<string>> notifications = new();
        #endregion

        #region Properties
        public SnackbarMessageQueue MessageQueue => messageQueue;
        #endregion

        #region Constructors
        public SnackbarMessageService(SnackbarMessageQueue messageQueue)
        {
            this.messageQueue = messageQueue;
        }
        #endregion

        #region Methods
        public void EnqueueMessage(string message)
        {
            messageQueue.Enqueue(message, null, null, null, false, true, TimeSpan.FromSeconds(1));
        }

        public void Subscribe(NotificationContext notificationType, Action<string> callback)
        {
            if (!notifications.ContainsKey(notificationType))
            {
                notifications[notificationType] = callback;
            }
        }

        public void Unsubscribe(NotificationContext notificationType)
        {
            notifications.Remove(notificationType);
        }

        public void TryNotify(NotificationContext notificationType, string message)
        {
            if (notifications.TryGetValue(notificationType, out var callback))
            {
                callback(message);
            }
        }

        #endregion

        #region Events
        #endregion
    }


    //public void RegisterNotification(Action<string> notificationAction, NotificationContext context)
    //{
    //    notifications[context] = notifications.TryGetValue(context, out var existingAction)
    //                             ? existingAction + notificationAction
    //                             : notificationAction;
    //}

    //public void UnregisterSuccessNotification(Action<string> notificationAction, NotificationContext context)
    //{
    //    if (notifications.TryGetValue(context, out var existingAction))
    //    {
    //        var newAction = existingAction - notificationAction;
    //        if (newAction == null || newAction.GetInvocationList().Length == 0)
    //        {
    //            notifications.Remove(context);
    //            return;
    //        }
    //        else
    //        {
    //            notifications[context] = newAction;
    //        }
    //    }
    //}

    //public void Notify(NotificationContext context, string message)
    //{
    //    if (notifications.TryGetValue(context, out var action))
    //    {
    //        action(message);
    //    }
    //}
}
