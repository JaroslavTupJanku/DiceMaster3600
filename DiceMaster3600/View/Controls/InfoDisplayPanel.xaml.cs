using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DiceMaster3600.View.Controls
{
    /// <summary>
    /// Interaction logic for InfoDisplayPanel.xaml
    /// </summary>
    public partial class InfoDisplayPanel : UserControl
    {
        public Brush TargetGridBackground
        {
            get { return (Brush)GetValue(TargetGridBackgroundProperty); }
            set { SetValue(TargetGridBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TargetGridBackgroundProperty =
            DependencyProperty.Register("TargetGridBackground", typeof(Brush), typeof(InfoDisplayPanel), new PropertyMetadata(Brushes.Transparent));

        public string UnitText
        {
            get { return (string)GetValue(UnitTextProperty); }
            set { SetValue(UnitTextProperty, value); }
        }

        public static readonly DependencyProperty UnitTextProperty =
            DependencyProperty.Register("UnitText", typeof(string), typeof(InfoDisplayPanel), new PropertyMetadata(" "));

        public string ValueText
        {
            get { return (string)GetValue(ValueTextProperty); }
            set { SetValue(ValueTextProperty, value); }
        }

        public static readonly DependencyProperty ValueTextProperty =
            DependencyProperty.Register("ValueText", typeof(string), typeof(InfoDisplayPanel), new PropertyMetadata("Unknow Value"));

        public string TitleText
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
        }

        public static readonly DependencyProperty TitleTextProperty =
            DependencyProperty.Register("TitleText", typeof(string), typeof(InfoDisplayPanel), new PropertyMetadata("Title"));

        public InfoDisplayPanel()
        {
            InitializeComponent();
        }
    }
}
