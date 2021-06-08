using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.DAL.Query;
using POS.DAL.Domain;
using POS.WPF.Models.ViewModels;
using System.Windows;
using POS.WPF.Common;

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

            var window = ServiceProvider.GetRequiredService<MainWindow>();
            window.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<POSContext>(ServiceLifetime.Transient);
            services.AddScoped<UserQuery>();
            services.AddScoped<OptionQuery>();
            services.AddScoped<ProductQuery>();
            services.AddScoped<AccountQuery>();
            services.AddScoped<InvoiceQuery>();

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

            services.AddTransient<LoginWindow>();
            services.AddTransient<MainWindow>();
        }
    }
}
