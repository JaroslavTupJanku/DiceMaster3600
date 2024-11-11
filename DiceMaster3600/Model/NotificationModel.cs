using CommunityToolkit.Mvvm.ComponentModel;
using DiceMaster3600.Core.Enum;

namespace DiceMaster3600.Model
{
    public class NotificationModel : ObservableObject
    {
        public string Text { get; set; }
        public GameNotificationType Type { get; set; }

        public NotificationModel(string text, GameNotificationType type)
        {
            Text = text;
            Type = type;
        }
    }
}
