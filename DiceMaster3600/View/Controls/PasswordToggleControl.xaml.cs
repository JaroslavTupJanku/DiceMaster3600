using DiceMaster3600.ViewModel.Control;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace DiceMaster3600.View.Controls
{
    /// <summary>
    /// Interaction logic for PasswordToggleControl.xaml
    /// </summary>
    public partial class PasswordToggleControl : UserControl
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                "Password",
                typeof(string),
                typeof(PasswordToggleControl),
                new PropertyMetadata(string.Empty, OnPasswordChanged)
            );

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public PasswordToggleControl()
        {
            DataContext = App.AppHost!.Services.GetService<PasswordToggleControlViewModel>();
            InitializeComponent();
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PasswordToggleControl)d;
            string newPassword = (string)e.NewValue;

            control.PasswordBox.Password = newPassword;
            control.VisiblePasswordBox.Text = newPassword;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            Password = passwordBox.Password;
        }
    }
}
