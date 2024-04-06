using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Model
{
    public interface INotificationService
    {
        void ShowToast(string message);
    }
}
