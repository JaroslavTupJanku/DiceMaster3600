using Intel.RealSense;

namespace DiceMaster3600.Model.FrameProcesses
{
    public class AverageDepthProcess : BaseResultFrameProcess<float>
    {
        private readonly int startX, startY, width, height;

        public AverageDepthProcess(int startX, int startY, int width, int height)
        {
            this.startX = startX;
            this.startY = startY;
            this.width = width;
            this.height = height;
        }

        protected override float ProcessFrame(FrameSet frameSet)
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

            return count > 0 ? totalDepth / count : 0;
        }

    }
}
