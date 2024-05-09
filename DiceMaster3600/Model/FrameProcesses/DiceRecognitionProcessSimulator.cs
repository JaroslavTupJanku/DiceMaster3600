using Intel.RealSense;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DiceMaster3600.Model.FrameProcesses
{
    public class DiceRecognitionProcessSimulator : BaseFrameProcess<List<CameraDiceResult>>
    {
        protected override BitmapSource ProcessFrame(FrameSet frameSet)
        {
            Result = new List<CameraDiceResult>();
            var random = new Random();

            int numberOfDices = random.Next(1, 6);
            for (int i = 0; i < numberOfDices; i++)
            {
                var position = new Int32Rect(random.Next(640), random.Next(480), 20, 20);
                var dotCount = random.Next(1, 7);
                var depth = (float)(random.NextDouble() * 100);

                Result.Add(new CameraDiceResult(position, dotCount, depth));
            }

            Thread.Sleep(1000);
            return null!;
        }
    }
}
