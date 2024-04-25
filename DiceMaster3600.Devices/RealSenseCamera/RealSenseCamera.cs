﻿using DiceMaster3600.Core;
using Intel.RealSense;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiceMaster3600.Devices.RealSenseCamera
{
    public class RealSenseCamera : IRealSenseCamera, IAsyncDisposable
    {
        private readonly Pipeline pipeline = new();
        private readonly List<IFrameProcces> frameprocesses = new();
        private readonly SemaphoreSlim processingSemaphore = new SemaphoreSlim(1, 1);
        private readonly CameraConfigurator configurator;

        private CancellationTokenSource cancellationTokenSource;

        public RealSenseCamera()
        {
            configurator = new CameraConfigurator(pipeline);
        }

        public async Task ConnectAsync()
        {
            var config = new Config();
            cancellationTokenSource = new CancellationTokenSource();
            config.EnableStream(Stream.Depth, 640, 480, Format.Z16, 30);
            config.EnableStream(Stream.Color, 640, 480, Format.Bgr8, 30);

            try
            {
                await Task.Run(() => pipeline.Start(config));
                _ = ProcessFramesAsync(cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                AppLogger.Error($"Error starting the RealSense pipeline: {ex.Message}");
            }
        }

        private async Task ProcessFramesAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    using var frames = pipeline.WaitForFrames();
                    OnNewFrame?.Invoke(this, frames);

                    if (await processingSemaphore.WaitAsync(0, token))
                    {
                        try
                        {
                            await ProcessFrameAsync(frames);
                        }
                        finally
                        {
                            processingSemaphore.Release();
                        }
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
                    await process.ProcessFrameAsync(frames);
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error($"Error processing frame asynchronously: {ex.Message}");
            }
        }

        public async Task DisconnectAsync()
        {
            try
            {
                cancellationTokenSource?.Cancel();
                await processingSemaphore.WaitAsync();
                pipeline.Stop();
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

        public void RegisterFrameProcessor(IFrameProcces processor)
        {
            lock (frameprocesses)
            {
                frameprocesses.Add(processor);
            }
        }

        public void UnregisterFrameProcessor(IFrameProcces processor)
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

        public event EventHandler<FrameSet> OnNewFrame;
    }
}
