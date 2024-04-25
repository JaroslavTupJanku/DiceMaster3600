using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Intel.RealSense;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

namespace DiceMaster3600.Model.FrameProcesses
{
    public class DiceRecognitionProcess : BaseResultFrameProcess<List<CameraDiceResult>>
    {
        private static readonly int MinimumContourWidth = 20;
        private static readonly int MinimumContourHeight = 20;
        private static readonly int DotThresholdArea = 10;
        private static readonly int ThresholdBinaryInvUpper = 255;
        private static readonly int ThresholdBinaryInvLower = 120;

        private readonly float backgroundDepthThreshold;
        private readonly double minArea;
        private readonly double minAspectRatio;
        private readonly double maxAspectRatio;
        private readonly double intensityThreshold;

        public DiceRecognitionProcess(float backgroundDepthThreshold, double intensityThreshold, double minArea, double minAspectRatio, double maxAspectRatio)
        {
            this.backgroundDepthThreshold = backgroundDepthThreshold;
            this.intensityThreshold = intensityThreshold;

            this.minArea = minArea;
            this.minAspectRatio = minAspectRatio;
            this.maxAspectRatio = maxAspectRatio;
        }

        protected override List<CameraDiceResult> ProcessFrame(FrameSet frameSet)
        {
            var results = new List<CameraDiceResult>();
            var colorImage = ConvertToColorImage(frameSet.ColorFrame);
            var depthImage = ConvertDepthFrameToImage(frameSet.DepthFrame);

            var diceRegions = DetectDiceByDepth(depthImage);
            foreach (var region in diceRegions)
            {
                var dotCount = CountDots(colorImage, region);
                var diceDepth = GetAverageDepth(frameSet.DepthFrame, region);
                results.Add(new CameraDiceResult(region, dotCount, diceDepth));
            }
            return results;
        }

        private List<Int32Rect> DetectDiceByDepth(Image<Gray, byte> depthImage)
        {
            using Mat mask = new();
            CvInvoke.Threshold(depthImage, mask, backgroundDepthThreshold, 255, ThresholdType.Binary);
            return DetectRegions(mask);
        }

        private List<Int32Rect> DetectDiceByColor(Image<Bgr, byte> colorImage, Bgr targetColor, double intenzityThresHold)
        {
            using Mat mask = new();
            CvInvoke.InRange(colorImage,
                new ScalarArray(GetLowerScalar(targetColor, intenzityThresHold)),
                new ScalarArray(GetUpperScalar(targetColor, intenzityThresHold)),
                mask);

            return DetectRegions(mask);
        }


        private List<Int32Rect> DetectRegions(Mat mask)
        {
            List<Int32Rect> regions = new();
            using var contours = new VectorOfVectorOfPoint();
            using var hierarchy = new Mat();

            CvInvoke.FindContours(mask, contours, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);
            for (int i = 0; i < contours.Size; i++)
            {
                using var contour = contours[i];
                Rectangle rect = CvInvoke.BoundingRectangle(contour);
                double area = CvInvoke.ContourArea(contour);
                double aspectRatio = (double)rect.Width / rect.Height;

                if (rect.Width > MinimumContourWidth && rect.Height > MinimumContourHeight && area > minArea && aspectRatio >= minAspectRatio && aspectRatio <= maxAspectRatio)
                {
                    regions.Add(new Int32Rect(rect.X, rect.Y, rect.Width, rect.Height));
                }
            }

            return regions;
        }

        private int CountDots(Image<Bgr, byte> colorImage, Int32Rect region)
        {
            int dotCount = 0;
            using VectorOfVectorOfPoint contours = new();
            using Mat hierarchy = new();

            using Image<Gray, byte> croppedImage = colorImage
                .Copy(new Rectangle(region.X, region.Y, region.Width, region.Height))
                .Convert<Gray, byte>()
                .ThresholdBinaryInv(new Gray(ThresholdBinaryInvLower), new Gray(ThresholdBinaryInvUpper));

            CvInvoke.FindContours(croppedImage, contours, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            for (int i = 0; i < contours.Size; i++)
            {
                if (CvInvoke.ContourArea(contours[i]) > DotThresholdArea) {
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

        private Image<Bgr, byte> ConvertToColorImage(VideoFrame colorFrame) => BitMapConverter.ConvertToImage<Bgr, byte>(colorFrame);
        private Image<Gray, byte> ConvertDepthFrameToImage(VideoFrame depthFrame) => BitMapConverter.ConvertDepthFrameToImage(depthFrame);

        private MCvScalar GetLowerScalar(Bgr target, double ct) => new(target.Blue - ct, target.Green - ct, target.Red - ct);
        private MCvScalar GetUpperScalar(Bgr target, double ct) => new(target.Blue + ct, target.Green + ct, target.Red + ct);
    }
}

