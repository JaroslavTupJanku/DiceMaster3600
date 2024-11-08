using DiceMaster3600.ViewModel;
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
            var v = App.AppHost!.Services.GetService<LoginFormViewModel>();
            DataContext = v;


            if (v.CloseAction == null)
            {
                v.CloseAction = new Action(this.Close);
            }

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
