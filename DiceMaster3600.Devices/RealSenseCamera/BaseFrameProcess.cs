using DiceMaster3600.Devices.RealSenseCamera;
using Emgu.CV;
using Intel.RealSense;
using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DiceMaster3600.Model.FrameProcesses
{
    public abstract class BaseFrameProcess<T> : IFrameResultProcess<T>
    {
        private T result;

        public T Result
        {
            get => result;
            protected set
            {
                if (!Equals(result, value))
                {
                    result = value;
                    OnResultChanged?.Invoke();
                }
            }
        }

        protected abstract BitmapSource ProcessFrame(FrameSet frameSet);

        public async Task<BitmapSource> ProcessFrameAsync(FrameSet frame)
        {
            try
            {
                return await Task.Run(() => ProcessFrame(frame));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Image handle error", ex);
            }
        }


        public event Action OnResultChanged;
    }
}
