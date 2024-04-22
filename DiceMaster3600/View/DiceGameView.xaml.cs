using DiceMaster3600.Model.Services;
using Intel.RealSense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiceMaster3600.View
{
    /// <summary>
    /// Interaction logic for DiceGameView.xaml
    /// </summary>
    public partial class DiceGameView : UserControl
    {
        //private RealSenseCamera camera;

        public DiceGameView()
        {
            InitializeComponent();

            // Inicializace kamery
            //camera = new RealSenseCamera();
            //camera.OnNewFrame += Camera_OnNewFrame; ; ; // Přihlášení k odběru události
        }

        private void Camera_OnNewFrame(object? sender, VideoFrame frame)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                BitmapSource bitmapSource = BitmapSource.Create(
                    frame.Width, frame.Height,
                    96, 96,
                    PixelFormats.Bgr24, null,
                    frame.Data, frame.Stride * frame.Height, frame.Stride);

                MyImage.Source = bitmapSource; // MyImage je Image kontrola ve vašem XAML
            });
        }


        public void ProcessColorFrame(VideoFrame frame)
        {

        }

        // Spustí kameru
        public void StartCamera()
        {
            //camera.StartAsync();
        }

        // Zastaví kameru
        public void StopCamera()
        {
            //camera.Stop();
        }

        // Čištění při zničení UserControl
        //protected override void OnUnloaded(object sender, RoutedEventArgs e)
        //{
        //    camera.OnNewFrame -= ProcessColorFrame;
        //    StopCamera();
        //    base.OnUnloaded(sender, e);
        //}

        private void StartCamera_Click(object sender, RoutedEventArgs e)
        {
            StartCamera();
        }

        private void StopCamera_Click(object sender, RoutedEventArgs e)
        {
            StopCamera();
        }
    }
}
