using System.Windows.Controls;

namespace POS.WPF.Pages
{
    public partial class Products : Page
    {
        public Products(ViewModels.ProductViewModel context)
        {
            InitializeComponent();
            DataContext = context;
        }
    }
}
