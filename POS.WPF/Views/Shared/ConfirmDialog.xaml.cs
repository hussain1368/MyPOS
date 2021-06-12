using POS.WPF.Models.ViewModels;
using System.Windows.Controls;

namespace POS.WPF.Views.Shared
{
    public partial class ConfirmDialog : UserControl
    {
        public ConfirmDialog(ConfirmDialogVM context)
        {
            InitializeComponent();
            DataContext = context;
        }
    }
}
