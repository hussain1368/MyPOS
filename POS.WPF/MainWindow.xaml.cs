using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace POS.WPF
{
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider serviceProvider;

        public Pages.Products ProductsPage { get; set; }
        public Pages.Home HomePage { get; set; }

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.serviceProvider = serviceProvider;
            GetPages();
        }

        private void GetPages()
        {
            ProductsPage = serviceProvider.GetRequiredService<Pages.Products>();
            HomePage = serviceProvider.GetRequiredService<Pages.Home>();
            MainFrame.Content = ProductsPage;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = SideMenu.SelectedItem as ListViewItem;
            MainFrame.Content = null;
            switch (selectedItem.Name)
            {
                case "Home": MainFrame.Navigate(HomePage); break;
                case "Products": MainFrame.Navigate(ProductsPage); break;
            }
        }
    }
}
