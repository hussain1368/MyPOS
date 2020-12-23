using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.DAL.DataQuery;
using POS.DAL.Domain;
using POS.WPF.ViewModels;
using System;
using System.Windows;

namespace POS.WPF
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<POSContext>();
            services.AddScoped<ProductQuery>();
            services.AddScoped<ProductContext>();

            services.AddTransient<MainWindow>();
            services.AddTransient<Pages.Products>();
            services.AddTransient<Pages.Home>();
        }
    }
}
