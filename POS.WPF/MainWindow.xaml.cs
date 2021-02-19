using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace POS.WPF
{
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider serviceProvider;

        public Pages.Home HomePage { get; set; }
        public Pages.Products ProductsPage { get; set; }
        public Pages.Accounts AccountsPage { get; set; }

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.serviceProvider = serviceProvider;
            GetPages();
        }

        private void GetPages()
        {
            HomePage = serviceProvider.GetRequiredService<Pages.Home>();
            ProductsPage = serviceProvider.GetRequiredService<Pages.Products>();
            AccountsPage = serviceProvider.GetRequiredService<Pages.Accounts>();
            MainFrame.Content = HomePage;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = SideMenu.SelectedItem as ListViewItem;
            MainFrame.Content = null;
            switch (selectedItem.Name)
            {
                case "Home": MainFrame.Navigate(HomePage); break;
                case "Products": MainFrame.Navigate(ProductsPage); break;
                case "Accounts": MainFrame.Navigate(AccountsPage); break;
            }
        }
    }
}
