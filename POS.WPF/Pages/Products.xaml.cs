using System.Windows.Controls;

namespace POS.WPF.Pages
{
    public partial class Products : Page
    {
        public Products(ViewModels.ProductVM context)
        {
            InitializeComponent();
            DataContext = context;
        }
    }
}
