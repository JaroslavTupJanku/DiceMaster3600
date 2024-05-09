using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Devices.RealSenseCamera;
using DiceMaster3600.Model.FrameProcesses;
using System.Collections.Generic;

namespace DiceMaster3600.Model.Services
{
    public class ProcessProvider : IProcessProvider, IProcessManager
    {
        private readonly List<IFrameProcess> processes = new();
        private readonly IRealSenseCamera camera;

        public IFrameResultProcess<int[]>? DiceRecognitionProcess { get; private set; }
        public IFrameResultProcess<List<CameraDiceResult>>? SimulationDiceRecognitionProcess { get; private set; }

        public ProcessProvider(IRealSenseCamera camera)
        {
            this.camera = camera;
            InitializeProcesses();
        }

        public void InitializeProcesses()
        {
            //SimulationDiceRecognitionProcess = new DiceRecognitionProcessSimulator();
            DiceRecognitionProcess = new DiceRecognitionProcess(120, 40, 0.8, 1.2);

            processes.Add(DiceRecognitionProcess);
            RegisterAllProcesses();
        }

        public void RegisterAllProcesses() => processes.ForEach(p => camera.RegisterFrameProcessor(p));
        public void UnregisterAllProcesses() => processes.ForEach(p => camera.UnregisterFrameProcessor(p));

        public void RegisterProcess(IFrameProcess process) => camera.RegisterFrameProcessor(process);
        public void UnregisterProcess(IFrameProcess process) => camera.UnregisterFrameProcessor(process);
    }
}
