using DiceMaster3600.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DiceMaster3600
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = App.AppHost!.Services.GetService<MainViewModel>();            
            InitializeComponent();
        }
    }
}
