using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
                    services.AddSingleton<ControlProvider>();
                    services.AddSingleton<OrderManager>();
                    services.AddSingleton<ShoppingCart>();
                    services.AddSingleton<IPizzaBuilder, PizzaBuilder>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<OrderViewModel>();
                    services.AddSingleton<OrderRepository>();

                    services.AddTransient<IControlViewModel, FavoritOrderViewModel>();
                    services.AddTransient<IControlViewModel, OrderHistoryViewModel>();
                    services.AddTransient<IControlViewModel, ShoppingCartViewModel>();
                    services.AddTransient<IControlViewModel, PizzaMenuViewModel>();
                    services.AddTransient<IngredientsViewModel>();
                }).Build();
        }
    }
}
