using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Intel.RealSense;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DiceMaster3600.Model.FrameProcesses
{
    public class DiceRecognitionProcess : BaseResultFrameProcess<List<DiceResult>>  //Napsat, ze by to bylo lepsi retezit. 
    {

        private readonly float backgroundDepthThreshold;
        private readonly double minArea;
        private readonly double minAspectRatio;
        private readonly double maxAspectRatio;
        private readonly double intenzityThresHold;

        public DiceRecognitionProcess(float backgroundDepthThreshold, double minArea, double minAspectRatio, double maxAspectRatio, double dotContourAreaThreshold)
        {
            this.backgroundDepthThreshold = backgroundDepthThreshold;
            this.minArea = minArea;
            this.minAspectRatio = minAspectRatio;
            this.maxAspectRatio = maxAspectRatio;
            this.intenzityThresHold = dotContourAreaThreshold;
        }

        protected override List<DiceResult> ProcessFrame(FrameSet frameSet)
        {
            var results = new List<DiceResult>();
            var colorFrame = frameSet.ColorFrame;
            var depthFrame = frameSet.DepthFrame;

            WriteableBitmap colorBitmap = BitMapConverter.ConvertToWriteableBitmap(colorFrame);
            Image<Bgr, byte> colorImage = BitMapConverter.ConvertWriteableBitmapToImage<Bgr, byte>(colorBitmap);
            Image<Gray, byte> depthImage = BitMapConverter.ConvertDepthFrameToImage(depthFrame);


            var diceRegions = DetectDiceByDepth(depthImage, backgroundDepthThreshold);

            foreach (var region in diceRegions)
            {
                var dotCount = CountDots(colorImage, region);
                var diceDepth = GetAverageDepth(depthFrame, region);

                results.Add(new DiceResult(region, dotCount, diceDepth));
            }
            return results;
        }

        private List<Int32Rect> DetectDiceByDepth(Image<Gray, byte> depthImage, float backgroundDepthThreshold)
        {
            using Mat mask = new Mat();
            CvInvoke.Threshold(depthImage, mask, backgroundDepthThreshold, 255, ThresholdType.Binary);
            return DetectRegions(mask);
        }

        private List<Int32Rect> DetectDiceByColor(Image<Bgr, byte> colorImage, Bgr targetColor, double intenzityThresHold)
        {
            using Mat mask = new Mat();
            CvInvoke.InRange(colorImage,
                new ScalarArray(GetLowerScalar(targetColor, intenzityThresHold)),
                new ScalarArray(GetUpperScalar(targetColor, intenzityThresHold)),
                mask);

            return DetectRegions(mask);
        }

        private List<Int32Rect> DetectRegions(Mat mask)
        {
            List<Int32Rect> regions = new List<Int32Rect>();
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();

            CvInvoke.FindContours(mask, contours, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            for (int i = 0; i < contours.Size; i++)
            {
                using VectorOfPoint contour = contours[i];
                Rectangle rect = CvInvoke.BoundingRectangle(contour);
                double area = CvInvoke.ContourArea(contour);
                double aspectRatio = (double)rect.Width / rect.Height;

                if (rect.Width > 20 && rect.Height > 20 && area > minArea && aspectRatio >= minAspectRatio && aspectRatio <= maxAspectRatio)
                {
                    regions.Add(new Int32Rect(rect.X, rect.Y, rect.Width, rect.Height));
                }
            }
            return regions;
        }

        private int CountDots(Image<Bgr, byte> colorImage, Int32Rect region)
        {
            VectorOfVectorOfPoint contours = new();
            int dotCount = 0;
            Mat hierarchy = new();

            Image<Gray, byte> croppedImage = colorImage.Copy(new Rectangle(region.X, region.Y, region.Width, region.Height))
                                                       .Convert<Gray, byte>().ThresholdBinaryInv(new Gray(120), new Gray(255));

            CvInvoke.FindContours(croppedImage, contours, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);
            for (int i = 0; i < contours.Size; i++)
            {
                if (CvInvoke.ContourArea(contours[i]) > 10)
                {
                    dotCount++;
                }
            }

            return dotCount;
        }

        private float GetAverageDepth(VideoFrame frame, Int32Rect region)
        {
            int[] depthData = new int[region.Width * region.Height];
            frame.CopyTo(depthData);

            long totalDepth = 0;
            int count = 0;

            foreach (int depth in depthData)
            {
                if (depth > 0)
                {
                    totalDepth += depth;
                    count++;
                }
            }
            return count > 0 ? (float)totalDepth / count : 0;
        }

        private MCvScalar GetLowerScalar(Bgr target, double ct) => new(target.Blue - ct, target.Green - ct, target.Red - ct);
        private MCvScalar GetUpperScalar(Bgr target, double ct) => new(target.Blue + ct, target.Green + ct, target.Red + ct);
    }
}

