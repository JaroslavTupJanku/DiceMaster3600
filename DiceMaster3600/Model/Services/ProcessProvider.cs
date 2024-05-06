using DiceMaster3600.Devices.RealSenseCamera;
using DiceMaster3600.Model.FrameProcesses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiceMaster3600.Model.Services
{
    public class ProcessProvider : IProcessProvider, IProcessManager
    {
        private readonly List<IFrameProcces> processes = new();
        private readonly IRealSenseCamera camera;

        public IResultFrameProcess<int[]>? DiceRecognitionProcess { get; private set; }
        public IResultFrameProcess<List<CameraDiceResult>>? SimulationDiceRecognitionProcess { get; private set; }

        public ProcessProvider(IRealSenseCamera camera)
        {
            this.camera = camera;
            InitializeProcesses();
        }

        public void InitializeProcesses()
        {
            //SimulationDiceRecognitionProcess = new DiceRecognitionProcessSimulator();
            DiceRecognitionProcess = new DiceRecognitionProcess(50, 50, 20, 20, 20);

            processes.Add(DiceRecognitionProcess);
            RegisterAllProcesses();
        }

        public void RegisterAllProcesses() => processes.ForEach(p => camera.RegisterFrameProcessor(p));
        public void UnregisterAllProcesses() => processes.ForEach(p => camera.UnregisterFrameProcessor(p));

        public void RegisterProcess(IFrameProcces process) => camera.RegisterFrameProcessor(process);
        public void UnregisterProcess(IFrameProcces process) => camera.UnregisterFrameProcessor(process);
    }
}
