using Emgu.CV;
using Intel.RealSense;
using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DiceMaster3600.Core.InterFaces
{
    public interface IFrameProcess
    {    
        Task<BitmapSource> ProcessFrameAsync(FrameSet frame);
    }
}
