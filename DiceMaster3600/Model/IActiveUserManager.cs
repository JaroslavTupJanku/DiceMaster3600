using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Model
{
    public interface IActiveUserManager
    {
        bool Logout();
        event EventHandler<bool> OnUserLogged;
    }
}
