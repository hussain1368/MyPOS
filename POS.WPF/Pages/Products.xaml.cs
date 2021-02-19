using System.Windows.Controls;

namespace POS.WPF.Pages
{
    public partial class Products : Page
    {
        public Products(ViewModels.ProductsVM context)
        {
            InitializeComponent();
            DataContext = context;
        }
    }
}
