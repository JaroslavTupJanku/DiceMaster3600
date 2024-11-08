using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Intel.RealSense;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;

namespace DiceMaster3600.Model.FrameProcesses
{
    public class DiceRecognitionProcess : BaseFrameProcess<int[]>
    {
        #region Fields
        private readonly int kernelSize = 5;

        private readonly double minArea;
        private readonly double minAspectRatio;
        private readonly double maxAspectRatio;
        private readonly double intensityThreshold;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public DiceRecognitionProcess(double intensityThreshold, double minArea, double minAspectRatio, double maxAspectRatio)
        {
            this.intensityThreshold = intensityThreshold;
            this.minArea = minArea;
            this.minAspectRatio = minAspectRatio;
            this.maxAspectRatio = maxAspectRatio;
        }
        #endregion

        #region Methods
        protected override BitmapSource ProcessFrame(FrameSet frameSet)
        {
            var colorImage = ConvertToColorImage(frameSet.ColorFrame);
            var depthImage = ConvertDepthFrameToImage(frameSet.DepthFrame);
            var dots = DetectDice(colorImage);

            foreach (var dot in dots)
            {
                double averageDepth = CalculateAverageDepth(dot, depthImage);
                string depthText = $"{Math.Round(averageDepth, 2)}mm";
                CvInvoke.PutText(colorImage, depthText, new Point(dot.X, dot.Y - 10), FontFace.HersheySimplex, 0.5, new Bgr(Color.Yellow).MCvScalar);

                var normalizedDepthImage = new Image<Gray, byte>(depthImage.Size);
                CvInvoke.Normalize(depthImage, normalizedDepthImage, 0, 255, NormType.MinMax);
            }

            Result = DetectDots(colorImage, dots).ToArray();
            return  BitMapConverter.ToBitmapSource(colorImage);
        }

        public List<Rectangle> DetectDice(Image<Bgr, byte> image)
        {
            var grayImage = image.Convert<Gray, byte>();
            grayImage = grayImage.SmoothGaussian(kernelSize);

            var binaryImage = grayImage.ThresholdBinary(new Gray(intensityThreshold), new Gray(255));
            var cannyEdges = binaryImage.Canny(100, 200);

            using VectorOfVectorOfPoint contours = new();
            CvInvoke.FindContours(cannyEdges, contours, new Mat(), RetrType.List, ChainApproxMethod.ChainApproxSimple);

            Image<Bgr, byte> contourImage = image.CopyBlank();
            List<Rectangle> dices = new();

            for (int i = 0; i < contours.Size; i++)
            {
                Rectangle dice = CvInvoke.BoundingRectangle(contours[i]);
                if (HasValidSize(dice, minArea) && !dices.Any(b => b.IntersectsWith(dice)))
                {
                    dices.Add(dice);
                    CvInvoke.Rectangle(image, dice, new Bgr(Color.Yellow).MCvScalar, 2);
                }
            }

            return dices;
        }

        public List<int> DetectDots(Image<Bgr, byte> image, List<Rectangle> dice)
        {
            List<Rectangle> dots = new();
            List<int> dotsCount = new();
            foreach (var die in dice)
            {

                Image<Bgr, byte> dieImage = image.GetSubRect(die);
                var grayDieImage = dieImage.Convert<Gray, byte>().ThresholdBinaryInv(new Gray(intensityThreshold), new Gray(255));

                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(grayDieImage, contours, new Mat(), RetrType.List, ChainApproxMethod.ChainApproxSimple);

                var dotcounter = 0;
                for (int i = 0; i < contours.Size; i++)
                {
                    if (contours[i].Size < 10 )  //TO motom yaloyit na min area asi.
                        continue;

                    Rectangle dot = CvInvoke.BoundingRectangle(contours[i]);
                    //if (HasValidShape(dot)
                    //    && !dots.Any(b => b.IntersectsWith(dot)
                    //    && HasCircularShape(contours[i]))
                    //    && IsInsideInnerRect(dot, innerRect)
                    //    && !IsInCorner(dot, die, offset))
                    //{

                    if (HasValidShape(dot) && IsInsideInnerRect(dot, die))
                    {
                        dot.Offset(die.Location);
                        dots.Add(dot);
                        dotcounter++;
                        CvInvoke.Rectangle(image, dot, new Bgr(Color.Red).MCvScalar, 1);
                    }
                    //}
                }
                dotsCount.Add(dotcounter);
            }

            return dotsCount;
        }

        private bool HasCircularShape(VectorOfPoint contour, double circularityThreshold = 0.9)
        {
            double area = CvInvoke.ContourArea(contour);
            if (area > 0) 
            {
                double perimeter = CvInvoke.ArcLength(contour, true);
                double circularity = (4 * Math.PI * area) / (perimeter * perimeter);
                return circularity > circularityThreshold;
            }
            return false;
        }

        private bool HasValidShape(Rectangle rect, double min = 0.5, double max = 1.5)
        {
            double aspectRatio = (double)rect.Width / rect.Height;
            return aspectRatio >= min && aspectRatio <= max;
        }

        private bool HasValidSize(Rectangle rect, double size, double min = 0.9, double max = 1.1)
        {
            return HasValidShape(rect, min, max)
                && rect.Width > size && rect.Height > size
                && rect.Width < 2*size && rect.Height < 2 * size;
        }

        private bool IsInsideInnerRect(Rectangle dot, Rectangle dice)
        {
            return dot.Left >= 4 && dot.Top >= 4 && dot.Right <= dice.Width - 4 && dot.Bottom <= dice.Height -4;
        }

        private bool IsInCorner(Rectangle dot, Rectangle die, int offset)
        {
            // Kontrola, zda je tečka v rozích kostky
            int cornerSize = offset;
            Rectangle topLeftCorner = new Rectangle(die.X, die.Y, cornerSize, cornerSize);
            Rectangle topRightCorner = new Rectangle(die.Right - cornerSize, die.Y, cornerSize, cornerSize);
            Rectangle bottomLeftCorner = new Rectangle(die.X, die.Bottom - cornerSize, cornerSize, cornerSize);
            Rectangle bottomRightCorner = new Rectangle(die.Right - cornerSize, die.Bottom - cornerSize, cornerSize, cornerSize);

            return topLeftCorner.IntersectsWith(dot) ||
                   topRightCorner.IntersectsWith(dot) ||
                   bottomLeftCorner.IntersectsWith(dot) ||
                   bottomRightCorner.IntersectsWith(dot);
        }

        public double CalculateAverageDepth(Rectangle diceBox, Image<Gray, ushort> depthImage)
        {
            double sumDepth = 0;
            int pixelCount = 0;
            for (int y = diceBox.Top; y < diceBox.Bottom; y++)
            {
                for (int x = diceBox.Left; x < diceBox.Right; x++)
                {
                    ushort depth = depthImage.Data[y, x, 0];
                    if (depth > 0)
                    {
                        sumDepth += depth;
                        pixelCount++;
                    }
                }
            }
            return pixelCount > 0 ? sumDepth / pixelCount : 0;
        }

        private Image<Bgr, byte> ConvertToColorImage(VideoFrame colorFrame) => BitMapConverter.ConvertToImage<Bgr, byte>(colorFrame);
        private Image<Gray, ushort> ConvertDepthFrameToImage(VideoFrame depthFrame) => BitMapConverter.ConvertToImage<Gray, ushort>(depthFrame);
        #endregion

        #region Events
        #endregion
    }
}

