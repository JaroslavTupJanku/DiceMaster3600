using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiceMaster3600.Model
{
    public class NotificationServise
    {
        private readonly Snackbar snackbar;

        public NotificationServise(Snackbar snackbar)
        {
            this.snackbar = snackbar;
        }

        public void ShowToast(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                snackbar.MessageQueue?.Enqueue(message);
            });
        }
    }
}
