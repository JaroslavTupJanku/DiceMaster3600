﻿using DiceMaster3600.Core;
using DiceMaster3600.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DiceMaster3600.Model
{
    public class ViewModelFactory : IViewModelFactory
    {
        public T CreateViewModel<T>() where T : class, IMenuControlViewModel
        {
            return App.AppHost!.Services?.GetService<T>()
                ?? HandleError<T>(); 
        }

        private static T HandleError<T>() where T : class
        {
            var errorMessage = $"ViewModel of type {typeof(T).Name} could not be created.";
            AppLogger.Error(errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }
}
