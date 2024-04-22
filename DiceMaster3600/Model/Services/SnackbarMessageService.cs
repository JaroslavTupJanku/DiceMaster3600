using DiceMaster3600.Core.Enum;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DiceMaster3600.Model.Services
{
    public class SnackbarMessageService : IMessageService
    {
        #region Fields
        private readonly SnackbarMessageQueue messageQueue;
        private readonly ConcurrentDictionary<NotificationContext, List<Action<string>>> notifications = new();
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
        public void Subscribe(NotificationContext notificationType, Action<string> callback)
        {
            notifications.AddOrUpdate(notificationType, new List<Action<string>> { callback }, (key, oldValue) =>
            {
                lock (oldValue) { oldValue.Add(callback); }
                return oldValue;
            });
        }

        public void Unsubscribe(NotificationContext notificationType)
        {
            if (notifications.TryRemove(notificationType, out var callbacks))
            {
                lock (callbacks) 
                { 
                    callbacks.Clear(); 
                }
            }
        }

        public void TryNotify(NotificationContext notificationType, string message)
        {
            if (notifications.TryGetValue(notificationType, out var callbacks))
            {
                lock (callbacks) 
                { 
                    callbacks.ForEach(cb => cb(message)); 
                }
            }
        }

        public void NotifyImmediately(string message)
        {
            messageQueue.Enqueue(message, null, null, null, false, true, TimeSpan.FromSeconds(1));
        }
        #endregion

        #region Events
        #endregion
    }
}
