using DiceMaster3600.ViewModel.Control;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiceMaster3600.View.Controls
{
    /// <summary>
    /// Interaction logic for YahtzeeTable.xaml
    /// </summary>
    public partial class YahtzeeTable : UserControl
    {
        public YahtzeeTable()
        {
            DataContext = App.AppHost!.Services.GetService<YahtzeeViewModel>();
            InitializeComponent();
        }
    }
}
