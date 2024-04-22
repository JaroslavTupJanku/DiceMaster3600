using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.Core.InterFaces
{
    public interface IDevice
    {
        Task ConnectAsync();
        void Disconnect();
    }
}
