using Intel.RealSense;
using System;
using System.Threading.Tasks;

namespace DiceMaster3600.Devices.RealSenseCamera
{
    public interface IFrameProcces
    {
        Task ProcessFrameAsync(FrameSet frame);

        event Action OnResultChanged;
    }
}
