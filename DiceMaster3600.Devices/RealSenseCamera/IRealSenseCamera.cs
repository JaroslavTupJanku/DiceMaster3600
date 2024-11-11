using DiceMaster3600.Core.InterFaces;
using System;
using System.Windows.Media.Imaging;

namespace DiceMaster3600.Devices.RealSenseCamera
{
    public interface IRealSenseCamera : IDevice
    {
        void RegisterFrameProcessor(IFrameProcess processor);
        void UnregisterFrameProcessor(IFrameProcess processor);

        event EventHandler<BitmapSource> OnNewFrame;
    }
}
