using DiceMaster3600.Core.InterFaces;
using Intel.RealSense;
using System;
using System.Windows.Media.Imaging;

namespace DiceMaster3600.Devices.RealSenseCamera
{
    public interface IRealSenseCamera : IDevice
    {
        void RegisterFrameProcessor(IFrameProcces processor);
        void UnregisterFrameProcessor(IFrameProcces processor);

        event EventHandler<FrameSet> OnNewFrame;
    }
}
