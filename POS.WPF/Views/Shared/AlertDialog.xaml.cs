using POS.WPF.Models.ViewModels;
using System.Windows.Controls;

namespace POS.WPF.Views.Shared
{
    public partial class AlertDialog : UserControl
    {
        public AlertDialog(MyDialogVM context)
        {
            InitializeComponent();
            DataContext = context;
        }
    }
}
