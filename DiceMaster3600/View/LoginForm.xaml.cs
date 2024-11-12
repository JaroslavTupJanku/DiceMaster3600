using DiceMaster3600.ViewModel.Control;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace DiceMaster3600.View
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
            var viewModel = App.AppHost!.Services.GetService<LoginFormViewModel>();
            DataContext = viewModel;


            viewModel!.CloseAction ??= new Action(Close);

            Closed += (sender, args) =>
            {
                if (this.DataContext is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            };
        }
    }
}
