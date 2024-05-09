using DiceMaster3600.Devices.RealSenseCamera;
using DiceMaster3600.Model.FrameProcesses;
using System.Collections.Generic;

namespace DiceMaster3600.Model.Services
{
    public interface IProcessProvider
    {
        IFrameResultProcess<int[]>? DiceRecognitionProcess { get; }
        IFrameResultProcess<List<CameraDiceResult>>? SimulationDiceRecognitionProcess { get; }
    }
}
