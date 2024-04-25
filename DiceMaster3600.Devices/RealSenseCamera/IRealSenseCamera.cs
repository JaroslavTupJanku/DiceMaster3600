using DiceMaster3600.Core.InterFaces;

namespace DiceMaster3600.Devices.RealSenseCamera
{
    public interface IRealSenseCamera : IDevice
    {
        void RegisterFrameProcessor(IFrameProcces processor);
        void UnregisterFrameProcessor(IFrameProcces processor);
    }
}
