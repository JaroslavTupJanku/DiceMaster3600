using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DiceMaster3600.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public HomeViewModel HomeVM { get; set; } 
        public object CurrentView { get; private set; }

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            CurrentView = HomeVM;
        }
    }
}
