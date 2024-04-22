using Emgu.CV;
using Emgu.CV.Structure;
using Intel.RealSense;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DiceMaster3600.Model.FrameProcesses
{
    public static class BitMapConverter
    {
        public static WriteableBitmap ConvertToWriteableBitmap(VideoFrame frame)
        {
            WriteableBitmap bitmap = new(frame.Width, frame.Height, 96, 96, PixelFormats.Bgr24, null);

            bitmap.Lock();
            frame.CopyTo(bitmap.BackBuffer);
            bitmap.AddDirtyRect(new Int32Rect(0, 0, frame.Width, frame.Height));
            bitmap.Unlock();

            return bitmap;
        }

        public static Image<Gray, byte> ConvertDepthFrameToImage(VideoFrame depthFrame)
        {
            int width = depthFrame.Width;
            int height = depthFrame.Height;

            // Předpokládáme, že hloubkový snímek je v 16bitovém formátu
            short[] depthData = new short[width * height];
            depthFrame.CopyTo(depthData);

            // Převedení dat na 8bitový formát pro jednodušší zpracování
            byte[] imageData = new byte[width * height];
            for (int i = 0; i < depthData.Length; i++)
            {
                // Převod 16bitové hloubky na 8bitový šedotónový obraz, můžete přizpůsobit měřítko
                imageData[i] = (byte)(depthData[i] / 256);
            }

            Image<Gray, byte> depthImage = new(width, height);
            depthImage.Bytes = imageData;

            return depthImage;
        }


        public static Image<TColor, TDepth> ConvertWriteableBitmapToImage<TColor, TDepth>(WriteableBitmap bitmap) where TColor : struct, IColor where TDepth : new()
        {
            bitmap.Lock();

            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int stride = bitmap.BackBufferStride;
            byte[] data = new byte[stride * height];

            Marshal.Copy(bitmap.BackBuffer, data, 0, data.Length);
            Image<TColor, TDepth> image = new(width, height);

            if (typeof(TColor) == typeof(Bgr) && typeof(TDepth) == typeof(byte) && image.Data is byte[,,] byteArray)
            {
                for (int y = 0; y < height; y++)
                {
                    int sourceIndex = y * stride;
                    for (int x = 0; x < width; x++)
                    {
                        byteArray[y, x, 0] = data[sourceIndex + x * 4 + 0];
                        byteArray[y, x, 1] = data[sourceIndex + x * 4 + 1];
                        byteArray[y, x, 2] = data[sourceIndex + x * 4 + 2];
                    }
                }
            }

            bitmap.Unlock();
            return image;
        }
    }
}
