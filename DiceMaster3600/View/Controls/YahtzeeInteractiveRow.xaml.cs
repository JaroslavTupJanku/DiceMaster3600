using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiceMaster3600.View.Controls
{
    /// <summary>
    /// Interaction logic for YahtzeeInteractiveRow.xaml
    /// </summary>
    public partial class YahtzeeInteractiveRow : UserControl
    {
        public YahtzeeInteractiveRow()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SelectCategoryCommandProperty =
            DependencyProperty.Register("SelectCategoryCommand", typeof(ICommand), typeof(YahtzeeInteractiveRow), new PropertyMetadata(null));

        public ICommand SelectCategoryCommand
        {
            get { return (ICommand)GetValue(SelectCategoryCommandProperty); }
            set { SetValue(SelectCategoryCommandProperty, value); }
        }


        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(YahtzeeInteractiveRow), new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }


        public static readonly DependencyProperty ScoreNameProperty =
            DependencyProperty.Register("ScoreName", typeof(string), typeof(YahtzeeInteractiveRow), new PropertyMetadata(string.Empty));

        public string ScoreName
        {
            get { return (string)GetValue(ScoreNameProperty); }
            set { SetValue(ScoreNameProperty, value); }
        }

        public static readonly DependencyProperty HowToScoreProperty =
            DependencyProperty.Register("HowToScore", typeof(string), typeof(YahtzeeInteractiveRow), new PropertyMetadata(string.Empty));

        public string HowToScore
        {
            get { return (string)GetValue(HowToScoreProperty); }
            set { SetValue(HowToScoreProperty, value); }
        }

        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(string), typeof(YahtzeeInteractiveRow), new PropertyMetadata(string.Empty));

        public string Score
        {
            get { return (string)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }

        public static readonly DependencyProperty FirstColumnWidthProperty =
            DependencyProperty.Register("FirstColumnWidth", typeof(int), typeof(YahtzeeInteractiveRow), new PropertyMetadata(20));

        public int FirstColumnWidth
        {
            get { return (int)GetValue(FirstColumnWidthProperty); }
            set { SetValue(FirstColumnWidthProperty, value); }
        }

        public static readonly DependencyProperty SecondColumnWidthProperty =
            DependencyProperty.Register("SecondColumnWidth", typeof(int), typeof(YahtzeeInteractiveRow), new PropertyMetadata(20));

        public int SecondColumnWidth
        {
            get { return (int)GetValue(SecondColumnWidthProperty); }
            set { SetValue(SecondColumnWidthProperty, value); }
        }

        public static readonly DependencyProperty ThirdColumnWidthProperty =
            DependencyProperty.Register("ThirdColumnWidth", typeof(int), typeof(YahtzeeInteractiveRow), new PropertyMetadata(20));

        public int ThirdColumnWidth
        {
            get { return (int)GetValue(ThirdColumnWidthProperty); }
            set { SetValue(ThirdColumnWidthProperty, value); }
        }
    }
}
