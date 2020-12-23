using System.Windows.Controls;

namespace POS.WPF.Pages
{
    public partial class Products : Page
    {
        public Products(ViewModels.ProductContext context)
        {
            InitializeComponent();
            DataContext = context;
        }
    }
}
