using Emgu.CV.Structure;
using Intel.RealSense;
using System.Windows.Media.Imaging;

namespace DiceMaster3600.Model.FrameProcesses
{
    public class AverageDepthProcess : BaseFrameProcess<float>
    {
        private readonly int startX, startY, width, height;

        public AverageDepthProcess(int startX, int startY, int width, int height)
        {
            this.startX = startX;
            this.startY = startY;
            this.width = width;
            this.height = height;
        }

        protected override BitmapSource ProcessFrame(FrameSet frameSet)
        {
            float totalDepth = 0;
            int count = 0;

            for (int y = startY; y < startY + height; y++)
            {
                for (int x = startX; x < startX + width; x++)
                {
                    float depth = frameSet.DepthFrame.GetDistance(x, y);
                    if (depth > 0)
                    {
                        totalDepth += depth;
                        count++;
                    }
                }
            }
            
            Result = count > 0 ? totalDepth / count : 0;
            var image = BitMapConverter.ConvertToImage<Gray, ushort>(frameSet.DepthFrame);
            return BitMapConverter.ToBitmapSource(image);
        }

    }
}
