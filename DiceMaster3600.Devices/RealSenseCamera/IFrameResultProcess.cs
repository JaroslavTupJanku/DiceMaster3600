using DiceMaster3600.Core.InterFaces;
using System;

namespace DiceMaster3600.Devices.RealSenseCamera
{
    public interface IFrameResultProcess<T> : IFrameProcess
    {
        T Result { get; }

        event Action OnResultChanged;
    }
}
