using DiceMaster3600.ViewModel;

namespace DiceMaster3600.Model
{
    public interface IViewModelFactory
    {
        T CreateViewModel<T>() where T : class, IMenuControlViewModel;
    }
}
