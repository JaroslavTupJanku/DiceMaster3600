using Emgu.CV;

namespace DiceMaster3600.Devices.RealSenseCamera
{
    public interface IAutoFrameResultProcess<T, TColor, TDepth> : IFrameResultProcess<T>
        where TColor : struct, IColor
        where TDepth : new()
    {

    }
}
