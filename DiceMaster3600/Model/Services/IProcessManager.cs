using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Devices.RealSenseCamera;

namespace DiceMaster3600.Model.Services
{
    public interface IProcessManager
    {
        void RegisterProcess(IFrameProcess process);
        void UnregisterProcess(IFrameProcess process);
        void RegisterAllProcesses();
        void UnregisterAllProcesses();
    }
}
