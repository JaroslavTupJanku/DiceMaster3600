using CommunityToolkit.Mvvm.ComponentModel;
using DiceMaster3600.Core.InterFaces;

namespace DiceMaster3600.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public HomeViewModel HomeVM { get; set; }
        public object CurrentView { get; private set; }

        public MainViewModel(IDataAccessManager manager)
        {
            HomeVM = new HomeViewModel(manager);
            CurrentView = HomeVM;
        }
    }
}
