using Intel.RealSense;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace DiceMaster3600.Model.FrameProcesses
{
    public class DiceRecognitionProcessSimulator : BaseResultFrameProcess<List<CameraDiceResult>>
    {
        protected override List<CameraDiceResult> ProcessFrame(FrameSet frameSet)
        {
            var results = new List<CameraDiceResult>();
            var random = new Random();

            int numberOfDices = random.Next(1, 6);
            for (int i = 0; i < numberOfDices; i++)
            {
                var position = new Int32Rect(random.Next(640), random.Next(480), 20, 20);
                var dotCount = random.Next(1, 7);
                var depth = (float)(random.NextDouble() * 100);

                results.Add(new CameraDiceResult(position, dotCount, depth));
            }

            Thread.Sleep(1000);
            return results;
        }
    }
}
