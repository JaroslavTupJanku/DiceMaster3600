using DiceMaster3600.Devices.RealSenseCamera;
using DiceMaster3600.Model.FrameProcesses;
using System.Collections.Generic;

namespace DiceMaster3600.Model.Services
{
    public interface IProcessProvider
    {
        IResultFrameProcess<List<CameraDiceResult>>? DiceRecognitionProcess { get; }
        IResultFrameProcess<List<CameraDiceResult>>? SimulationDiceRecognitionProcess { get; }
    }
}
