using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel.Control
{
    class PasswordToggleControlViewModel : ObservableObject
    {
        private bool isPasswordHidden = true;

        public bool IsPasswordHidden
        {
            get => isPasswordHidden;
            private set => SetProperty(ref isPasswordHidden, value);
        }

        public ICommand TogglePasswordVisibilityCommand { get; }

        public PasswordToggleControlViewModel()
        {
            TogglePasswordVisibilityCommand = new RelayCommand(
                () => IsPasswordHidden = !IsPasswordHidden);
        }
    }
}
