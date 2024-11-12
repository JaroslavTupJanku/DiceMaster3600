using DiceMaster3600.Core;
using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Data;
using DiceMaster3600.Devices.RealSenseCamera;
using DiceMaster3600.Model;
using DiceMaster3600.Model.Services;
using DiceMaster3600.Model.Yahtzee;
using DiceMaster3600.ViewModel;
using DiceMaster3600.ViewModel.Control;
using Intel.RealSense;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using System.Windows.Media.Media3D;

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
            AppLogger.ConfigureLogger();
            Exit += App_Exit;

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {

                    services.AddSingleton<RepositoryFasade>();
                    services.AddSingleton<ISqlRepositories, SqlRepositories>();
                    services.AddSingleton<IDataAccessManager, DataAccessManager>();
                    services.AddSingleton<IActiveUserManager, ActiveUserManager>();

                    services.AddSingleton<SnackbarMessageQueue>();
                    services.AddSingleton<GraphGenerator>();
                    services.AddSingleton<IViewModelFactory, ViewModelFactory>();
                    services.AddSingleton<IMessageService, NotificationService>();
                    services.AddSingleton<IRealSenseCamera, RealSenseCamera>();
                    services.AddSingleton<IYahtzeeScoreCounter, YahtzeeScoreCounter>();

                    services.AddSingleton<ProcessProvider>();
                    services.AddSingleton<IProcessProvider>(sp => sp.GetRequiredService<ProcessProvider>());
                    services.AddSingleton<IProcessManager>(sp => sp.GetRequiredService<ProcessProvider>());

                    services.AddSingleton<HomeViewModel>();
                    services.AddTransient<DiceGameViewModel>();
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<PasswordToggleControlViewModel>();
                    services.AddTransient<LoginFormViewModel>(); 
                    services.AddTransient<EntryFormViewModel>();
                    services.AddTransient<YahtzeeViewModel>();
                    services.AddTransient<AdminPanelViewModel>();

                }).Build();
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            var camera = App.AppHost!.Services.GetService<IRealSenseCamera>();
            camera?.DisconnectAsync();
        }
    }
}
