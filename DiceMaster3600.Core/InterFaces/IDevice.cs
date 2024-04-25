using System.Threading.Tasks;

namespace DiceMaster3600.Core.InterFaces
{
    public interface IDevice
    {
        Task ConnectAsync();
        Task DisconnectAsync();
    }
}
