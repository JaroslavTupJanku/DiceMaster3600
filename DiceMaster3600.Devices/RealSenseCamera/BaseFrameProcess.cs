using DiceMaster3600.Devices.RealSenseCamera;
using Intel.RealSense;
using System;
using System.Threading.Tasks;


namespace DiceMaster3600.Model.FrameProcesses
{
    public abstract class BaseFrameProcess<T> : IResultFrameProcess<T>
    {
        private T result;

        public T Result 
        { 
            get => result; 
            private set
            {
                result = value;
                OnResultChanged?.Invoke();
            }               
        }

        protected abstract T ProcessFrame(FrameSet frameSet);

        public async Task ProcessFrameAsync(FrameSet frameSet)
        {
            Result = await Task.Run(() => ProcessFrame(frameSet));
        }

        public event Action? OnResultChanged;
    }
}
