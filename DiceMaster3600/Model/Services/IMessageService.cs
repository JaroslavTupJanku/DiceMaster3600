using DiceMaster3600.Core.Enum;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Model.Services
{
    public interface IMessageService
    {
        SnackbarMessageQueue MessageQueue { get; }
        void EnqueueMessage(string message);
        void Subscribe(NotificationContext notificationType, Action<string> callback);
        void Unsubscribe(NotificationContext notificationType);
        void TryNotify(NotificationContext notificationType, string message);
    }
}
