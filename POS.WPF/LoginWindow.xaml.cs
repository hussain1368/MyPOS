using System.Windows;

namespace POS.WPF
{
    public partial class LoginWindow : Window
    {
        public LoginWindow(Models.ViewModels.LoginVM context)
        {
            InitializeComponent();
            DataContext = context;
        }
    }
}
