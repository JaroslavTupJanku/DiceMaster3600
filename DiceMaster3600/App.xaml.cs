﻿using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Data;
using DiceMaster3600.Data.Adapter;
using DiceMaster3600.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace DiceMaster3600
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IDataAccessManager, DataAccessManager>();
                    services.AddSingleton<RepositoryAdapter>();
                    services.AddSingleton<ISqlRepositories, SqlRepositories>();

                }).Build();
        }
    }
}
