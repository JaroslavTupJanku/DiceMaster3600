using DiceMaster3600.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DiceMaster3600.Model
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ViewModelFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public T CreateViewModel<T>() where T : class, IMenuControlViewModel
        {
            return serviceProvider.GetRequiredService<T>();
        }
    }
}
