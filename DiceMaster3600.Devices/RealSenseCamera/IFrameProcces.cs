using Intel.RealSense;
using System.Threading.Tasks;

namespace DiceMaster3600.Devices.RealSenseCamera
{
    public interface IFrameProcces
    {
        Task<FrameSet> ProcessFrameAsync(FrameSet frame);
    }
}
