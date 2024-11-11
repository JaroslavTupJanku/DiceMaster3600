using DiceMaster3600.Core;
using DiceMaster3600.Core.InterFaces;
using Intel.RealSense;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DiceMaster3600.Devices.RealSenseCamera
{

    public class RealSenseCamera : IRealSenseCamera, IAsyncDisposable
    {
        private readonly Pipeline pipeline = new();
        private readonly List<IFrameProcess> frameprocesses = new();
        private readonly SemaphoreSlim processingSemaphore = new SemaphoreSlim(1, 1);

        private readonly CameraConfigurator configurator;
        private CancellationTokenSource cancellationTokenSource;

        public bool IsConnected { get; private set; }

        public RealSenseCamera()
        {
            configurator = new CameraConfigurator(pipeline);
        }

        public async Task ConnectAsync()
        {
            if (IsConnected) {
                return;
            }

            var config = new Config();
            cancellationTokenSource = new CancellationTokenSource();
            config.EnableStream(Stream.Depth, 640, 480, Format.Z16, 30);
            config.EnableStream(Stream.Color, 640, 480, Format.Bgr8, 30);

            try
            {
                await Task.Run(() => pipeline.Start(config));
                IsConnected = true;
                _ = Task.Run(() => ProcessFramesAsync(cancellationTokenSource.Token));
            }
            catch (Exception ex)
            {
                IsConnected = false;
                AppLogger.Error($"Error starting the RealSense pipeline: {ex.Message}");
            }
        }

        private async Task ProcessFramesAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var frameset = pipeline.WaitForFrames();
                    frameset.Keep();

                    if (await processingSemaphore.WaitAsync(0, token))
                    {
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                await ProcessFrameAsync(frameset);
                            }
                            finally
                            {
                                frameset.Dispose();
                                processingSemaphore.Release();
                            }
                        }, token);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                AppLogger.Information($"{DateTime.Now} : Processing frames has been canceled in");
            }
            catch (Exception ex)
            {
                AppLogger.Error($"Error in processing frames: {ex.Message}");
            }
        }

        private async Task ProcessFrameAsync(FrameSet frames)
        {
            try
            {
                foreach (var process in frameprocesses)
                {
                    var processedFrame = await process.ProcessFrameAsync(frames);
                    OnNewFrame?.Invoke(this, processedFrame);
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error($"Error processing frame asynchronously: {ex.Message}");
            }
        }

        public async Task DisconnectAsync()
        {
            if (!IsConnected) {
                return;
            }

            try
            {
                cancellationTokenSource?.Cancel();
                await processingSemaphore.WaitAsync();
                pipeline.Stop();
                IsConnected = false;
            }
            catch (Exception ex)
            {
                AppLogger.Error($"Error disconnecting RealSense Camera: {ex.Message}");
            }
            finally
            {
                processingSemaphore.Release();
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }

        public void RegisterFrameProcessor(IFrameProcess processor)
        {
            lock (frameprocesses)
            {
                frameprocesses.Add(processor);
            }
        }

        public void UnregisterFrameProcessor(IFrameProcess processor)
        {
            lock (frameprocesses)
            {
                frameprocesses.Remove(processor);
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisconnectAsync();
            processingSemaphore.Dispose();
        }

        public event EventHandler<BitmapSource> OnNewFrame;
    }
}
