using DiceMaster3600.Model.Services;
using System.Runtime.CompilerServices;

namespace DiceMaster3600.ViewModel
{
    public abstract class ReactiveViewModel : NotifyViewModel
    {
        protected ReactiveViewModel(IMessageService messageService) : base(messageService){
        }

        protected bool SetTrigger<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (SetProperty(ref storage, value, propertyName))
            {
                RefreshCommand();
                return true;
            }
            return false;
        }

        abstract public void RefreshCommand();
    }
}
