using Material.Icons;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DiceMaster3600.View.Controls
{
    /// <summary>
    /// Interaction logic for RoundedIcon.xaml
    /// </summary>
    public partial class RoundedIcon : UserControl
    {
        public RoundedIcon()
        {
            InitializeComponent();
        }

        public Brush EllipseFill
        {
            get { return (Brush)GetValue(EllipseFillProperty); }
            set { SetValue(EllipseFillProperty, value); }
        }

        public static readonly DependencyProperty EllipseFillProperty =
            DependencyProperty.Register("EllipseFill", typeof(Brush), typeof(RoundedIcon), new PropertyMetadata(Brushes.White));

        public MaterialIconKind IconKind
        {
            get { return (MaterialIconKind)GetValue(IconKindProperty); }
            set { SetValue(IconKindProperty, value); }
        }

        public static readonly DependencyProperty IconKindProperty =
            DependencyProperty.Register("IconKind", typeof(MaterialIconKind), typeof(RoundedIcon), new PropertyMetadata(MaterialIconKind.Home));

        public Brush IconForeground
        {
            get { return (Brush)GetValue(IconForegroundProperty); }
            set { SetValue(IconForegroundProperty, value); }
        }

        public static readonly DependencyProperty IconForegroundProperty =
            DependencyProperty.Register("IconForeground", typeof(Brush), typeof(RoundedIcon), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x21, 0x96, 0xF3))));


        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty SizeProperty = 
            DependencyProperty.Register("Size", typeof(double), typeof(RoundedIcon), new PropertyMetadata(50.0, OnSizeChanged));

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) //0.75 by slo pres converter klidne. 
        {
            if (d is RoundedIcon control)
            {
                control.Ellipse.Width = (double)e.NewValue;
                control.Ellipse.Height = (double)e.NewValue;
                control.MaterialIcon.Height = 0.6 * (double)e.NewValue;
            }
        }
    }
}
