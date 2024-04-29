using DiceMaster3600.Core;
using Emgu.CV;
using Emgu.CV.Structure;
using Intel.RealSense;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DiceMaster3600.Model.FrameProcesses
{
    public static class BitMapConverter
    {
        public static BitmapSource ConvertToBitMatSource(VideoFrame frame)
        {
            return BitmapSource.Create( frame.Width, frame.Height, 96, 96,
                PixelFormats.Bgr24, null,
                frame.Data, frame.Stride * frame.Height, 
                frame.Stride);
        }

        public static Image<Gray, byte> ConvertDepthFrameToImage(VideoFrame depthFrame)
        {
            try
            {
                int width = depthFrame.Width;
                int height = depthFrame.Height;
                byte[] depthData = new byte[width * height];
                depthFrame.CopyTo(depthData);

                Image<Gray, byte> image = new(width, height)
                {
                    Bytes = depthData
                };

                return image;
            }
            catch (Exception ex)
            {
                AppLogger.Error($"Failed to convert depth frame to image : {ex}");
                throw new InvalidOperationException($"Failed to convert depth frame to image: {ex}");
            }
        }

        public static Image<TColor, TDepth> ConvertToImage<TColor, TDepth>(VideoFrame frame)
            where TColor : struct, IColor
            where TDepth : new()
        {
            try
            {
                int width = frame.Width;
                int height = frame.Height;

                byte[] data = new byte[frame.Stride * height];
                frame.CopyTo(data);

                Image<TColor, TDepth> image = new(width, height);
                Buffer.BlockCopy(data, 0, image.Data, 0, data.Length);

                return image;
            }
            catch (Exception ex)
            {
                AppLogger.Error($"Failed to convert depth frame to image : {ex}");
                throw new InvalidOperationException($"Failed to convert depth frame to image: {ex}");
            }
        }
    }
}
