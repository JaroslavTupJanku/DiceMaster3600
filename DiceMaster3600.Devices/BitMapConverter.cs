using DiceMaster3600.Core;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Intel.RealSense;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DiceMaster3600.Model.FrameProcesses
{
    public static class BitMapConverter
    {
        public static Image<TColor, TDepth> ConvertToImage<TColor, TDepth>(VideoFrame frame)
            where TColor : struct, IColor
            where TDepth : new()
        {
            try
            {
                int width = frame.Width;
                int height = frame.Height;

                TDepth[] data = new TDepth[frame.Stride * height];
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

        public static BitmapSource ToBitmapSource<TColor, TDepth>(Image<TColor, TDepth> image)
            where TColor : struct, IColor
            where TDepth : new()
        {
            PixelFormat format = GetPixelFormat(typeof(TColor));
            int depth = Marshal.SizeOf(typeof(TDepth));
            int channels = image.NumberOfChannels;

            var pixelData = new byte[image.Width * image.Height * channels * depth];
            GCHandle handle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);
            try
            {
                IntPtr scan0 = Marshal.UnsafeAddrOfPinnedArrayElement(pixelData, 0);
                image.Mat.CopyTo(new Mat(image.Height, image.Width, DepthType.Cv8U, channels, scan0, image.Width * channels * depth));

                BitmapSource bitmap = BitmapSource.Create(image.Width, image.Height, 96.0, 96.0, format, null, pixelData, image.Width * channels * depth);
                bitmap.Freeze();
                return bitmap;
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
        }

        private static PixelFormat GetPixelFormat(Type colorType)
        {
            if (colorType == typeof(Bgr))
                return PixelFormats.Bgr24;
            else if (colorType == typeof(Gray))
                return PixelFormats.Gray8;
            else
                throw new ArgumentException("Unsupported color format");
        }
    }
}
