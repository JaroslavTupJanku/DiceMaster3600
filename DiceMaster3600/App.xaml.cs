using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Data;
using DiceMaster3600.Model;
using DiceMaster3600.Model.Services;
using DiceMaster3600.Model.Yahtzee;
using DiceMaster3600.ViewModel;
using DiceMaster3600.ViewModel.Control;
using Intel.RealSense;
using MaterialDesignThemes.Wpf;
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

                    services.AddSingleton<RepositoryFasade>();
                    services.AddSingleton<ISqlRepositories, SqlRepositories>();
                    services.AddSingleton<IDataAccessManager, DataAccessManager>();
                    services.AddSingleton<IActiveUserManager, ActiveUserManager>();

                    services.AddSingleton<SnackbarMessageQueue>();
                    services.AddSingleton<IViewModelFactory, ViewModelFactory>();
                    services.AddSingleton<IMessageService, SnackbarMessageService>();
                    services.AddSingleton<YahtzeeScoreManager>();

                    services.AddTransient<HomeViewModel>();
                    services.AddTransient<DiceGameViewModel>();
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<LoginFormViewModel>(); //vysvetlit, proc je to dobre pro unsubscirbe. 
                    services.AddTransient<EntryFormViewModel>();
                    services.AddTransient<YahtzeeViewModel>();

                }).Build();
        }
    }
}
