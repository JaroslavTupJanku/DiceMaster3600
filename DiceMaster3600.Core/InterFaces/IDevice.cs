using System.Threading.Tasks;

namespace DiceMaster3600.Core.InterFaces
{
    public interface IDevice
    {
        bool IsConnected { get; }

        Task ConnectAsync();
        Task DisconnectAsync();
    }
}
