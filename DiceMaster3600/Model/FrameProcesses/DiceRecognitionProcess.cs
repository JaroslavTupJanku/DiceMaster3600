using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Intel.RealSense;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using Point = System.Drawing.Point;

namespace DiceMaster3600.Model.FrameProcesses
{
    public class DiceRecognitionProcess : BaseResultFrameProcess<int[]>
    {
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

        protected override int[] ProcessFrame(FrameSet frameSet)
        {

            var results = new List<int>();
            var colorImage = ConvertToColorImage(frameSet.ColorFrame);

            var diceBoxes = DetectDice(colorImage);
            return results.ToArray();
        }

        public List<Rectangle> DetectDice(Image<Bgr, byte> image)
        {
            var grayImage = image.Convert<Gray, byte>();
            grayImage = grayImage.SmoothGaussian(5); 

            var binaryImage = grayImage.ThresholdBinary(new Gray(120), new Gray(255));
            var cannyEdges = binaryImage.Canny(100, 200);

            using VectorOfVectorOfPoint contours = new();
            Mat hierarchy = new();
            CvInvoke.FindContours(cannyEdges, contours, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            Image<Bgr, byte> contourImage = image.CopyBlank();
            CvInvoke.DrawContours(image, contours, -1, new MCvScalar(0, 255, 0));

            List<Rectangle> diceBoxes = new();
            for (int i = 0; i < contours.Size; i++)
            {
                Rectangle boundingBox = CvInvoke.BoundingRectangle(contours[i]);
                if (IsDiceShape(boundingBox) && !diceBoxes.Any(b => b.IntersectsWith(boundingBox)))
                {
                    diceBoxes.Add(boundingBox);
                    CvInvoke.Rectangle(image, boundingBox, new Bgr(Color.Red).MCvScalar, 2);
                }
            }

            CvInvoke.Imshow("Final Image with Contours", image);
            CvInvoke.WaitKey(0);

            return diceBoxes;
        }

        private bool IsDiceShape(Rectangle rect)
        {
            double aspectRatio = (double)rect.Width / rect.Height;
            return aspectRatio >= 0.8 && aspectRatio <= 1.2 && rect.Width > 10 && rect.Height > 10;
        }


        private Image<Bgr, byte> ConvertToColorImage(VideoFrame colorFrame) => BitMapConverter.ConvertToImage<Bgr, byte>(colorFrame);
        private Image<Gray, byte> ConvertDepthFrameToImage(VideoFrame depthFrame) => BitMapConverter.ConvertDepthFrameToImage(depthFrame);

        private MCvScalar GetLowerScalar(Bgr target, double ct) => new(target.Blue - ct, target.Green - ct, target.Red - ct);
        private MCvScalar GetUpperScalar(Bgr target, double ct) => new(target.Blue + ct, target.Green + ct, target.Red + ct);
    }
}

