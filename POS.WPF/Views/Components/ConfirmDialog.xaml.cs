using POS.WPF.ViewModels;
using System.Windows.Controls;

namespace POS.WPF.Views.Components
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
