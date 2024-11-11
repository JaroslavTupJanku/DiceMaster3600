using System.Windows;
using System.Windows.Shapes;

namespace DiceMaster3600.Model.FrameProcesses
{
    public class CameraDiceResult
    {
        public Int32Rect? Position { get; }
        public int DotCount { get; }
        public float Depth { get; }

        public CameraDiceResult(Int32Rect? position, int dotCount, float depth)
        {
            Position = position;
            DotCount = dotCount;
            Depth = depth;
        }
    }
}
