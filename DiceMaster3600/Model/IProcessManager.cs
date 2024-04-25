using DiceMaster3600.Devices.RealSenseCamera;

namespace DiceMaster3600.Model
{
    public interface IProcessManager
    {
        void RegisterProcess(IFrameProcces process);
        void UnregisterProcess(IFrameProcces process);
        void RegisterAllProcesses();
        void UnregisterAllProcesses();
    }
}
