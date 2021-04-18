using System.Windows;

namespace POS.WPF
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(ViewModels.LoginVM context)
        {
            InitializeComponent();
            DataContext = context;
        }
    }
}
