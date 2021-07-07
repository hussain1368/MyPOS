using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.DAL.Repository;
using POS.DAL.Domain;
using POS.WPF.Models.ViewModels;
using System.Windows;
using POS.WPF.Common;
using System.Globalization;
using System.Threading;

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

            var state = ServiceProvider.GetRequiredService<AppState>();
            state.LoadSettings();

            var window = ServiceProvider.GetRequiredService<MainWindow>();
            window.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<POSContext>(ServiceLifetime.Transient);
            services.AddScoped<UserRepository>();
            services.AddScoped<OptionRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<AccountRepository>();
            services.AddScoped<InvoiceRepository>();

            services.AddLogging();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddSingleton<AppState>();
            services.AddScoped<LoginVM>();
            services.AddScoped<MainVM>();
            services.AddScoped<HomeVM>();
            services.AddScoped<ProductsVM>();
            services.AddScoped<AccountsVM>();
            services.AddScoped<InvoicesVM>();
            services.AddScoped<InvoiceFormVM>();
            services.AddScoped<SettingsVM>();

            services.AddTransient<LoginWindow>();
            services.AddTransient<MainWindow>();
        }
    }
}
