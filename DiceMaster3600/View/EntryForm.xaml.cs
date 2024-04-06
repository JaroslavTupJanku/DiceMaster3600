using DiceMaster3600.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DiceMaster3600.View.Dialogs
{
    /// <summary>
    /// Interaction logic for EntryForm.xaml
    /// </summary>
    public partial class EntryForm : Window
    {
        public EntryForm()
        {
            DataContext = App.AppHost!.Services.GetService<EntryFormViewModel>();
            InitializeComponent();
        }
    }
}
