using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Model
{
    public class ActiveUserManager : IActiveUserManager
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public bool Logout()
        {

            OnUserLogged?.Invoke(this, true); 
            throw new NotImplementedException();

        }
        #endregion

        #region Events
        #endregion



        public event EventHandler<bool>? OnUserLogged;
    }
}
