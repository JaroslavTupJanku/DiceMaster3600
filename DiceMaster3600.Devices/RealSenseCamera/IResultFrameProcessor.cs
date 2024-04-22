using System.Threading.Tasks;

namespace DiceMaster3600.Devices.RealSenseCamera
{
    public interface IResultFrameProcess<T> : IFrameProcces
    {
        T Result { get; }
    }
}
