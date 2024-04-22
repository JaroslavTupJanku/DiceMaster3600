using System.Windows;
using System.Windows.Shapes;

namespace DiceMaster3600.Model
{
    public class DiceResult
    {
        public Int32Rect? Position { get; }
        public int DotCount { get;  }
        public float Depth { get; }

        public DiceResult(Int32Rect? position, int dotCount, float depth)
        {
            Position = position;
            DotCount = dotCount;
            Depth = depth;
        }
    }
}
