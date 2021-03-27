using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.DAL.Query;
using POS.DAL.Domain;
using POS.WPF.ViewModels;
using System;
using System.Windows;

namespace POS.WPF
{
    public partial class App : Application
    {
        public ServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            ServiceLocator.SetLocatorProvider(ServiceProvider);

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<POSContext>(ServiceLifetime.Transient);
            services.AddScoped<ProductQuery>();
            services.AddScoped<AccountQuery>();
            services.AddScoped<OptionQuery>();

            services.AddLogging();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddScoped<MainVM>();
            services.AddScoped<HomeVM>();
            services.AddScoped<ProductsVM>();
            services.AddScoped<AccountsVM>();

            services.AddTransient<MainWindow>();
        }
    }
}
