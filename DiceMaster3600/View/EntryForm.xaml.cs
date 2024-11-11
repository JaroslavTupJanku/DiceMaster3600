using DiceMaster3600.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            InitializeComponent();

            if (App.AppHost!.Services.GetService<EntryFormViewModel>() is EntryFormViewModel vm) 
            {
                DataContext = vm;
                vm.RequestClose += () => Close();
                Closed += (s, a) => vm.Dispose();
            }
        }
    }
}
