using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace DiceMaster3600
{
    public abstract class BaseViewModel : ObservableObject
    {
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
