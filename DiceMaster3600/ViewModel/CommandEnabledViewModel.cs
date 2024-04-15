using CommunityToolkit.Mvvm.ComponentModel;
using DiceMaster3600.Model.Services;
using System.Runtime.CompilerServices;

namespace DiceMaster3600.ViewModel
{
    public abstract class CommandNotifyViewModel : NotifyViewModel
    {
        
        protected CommandNotifyViewModel(IMessageService messageService) : base(messageService) {
        }

        protected bool SetAndNotify<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (SetProperty(ref storage, value, propertyName))
            {
                NotifyCommandCanExecuteChanged();
                return true;
            }
            return false;
        }

        abstract public void NotifyCommandCanExecuteChanged();
    }
}
