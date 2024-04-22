using DiceMaster3600.Core.InterFaces;
using Intel.RealSense;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiceMaster3600.Devices.RealSenseCamera
{
    public class RealSenseCamera : IDevice
    {
        private readonly Pipeline pipeline = new();
        private readonly object lockObject = new object();
        private readonly List<IFrameProcces> frameprocesses = new();

        private CancellationTokenSource cancellationTokenSource;

        public async Task ConnectAsync()
        {
            var config = new Config();
            cancellationTokenSource = new CancellationTokenSource();
            config.EnableStream(Stream.Depth, 640, 480, Format.Z16, 30);
            config.EnableStream(Stream.Color, 640, 480, Format.Bgr8, 30);

            pipeline.Start(config);
            await Task.Run(() => ProcessFrames(cancellationTokenSource.Token), cancellationTokenSource.Token);
        }

        private async Task ProcessFrames(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                using var frames = pipeline.WaitForFrames();
                foreach (var p in frameprocesses)
                {
                    await p.ProcessFrameAsync(frames);
                }

                OnNewFrame?.Invoke(this, frames);
            }
        }

        public void Disconnect()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
            pipeline.Stop();
        }

        public void RegisterFrameProcessor(IFrameProcces processor)
        {
            lock (lockObject)
            {
                frameprocesses.Add(processor);
            }
        }

        public void UnregisterFrameProcessor(IFrameProcces processor)
        {
            lock (lockObject)
            {
                frameprocesses.Remove(processor);
            }
        }

        public event EventHandler<FrameSet> OnNewFrame;
    }
}
