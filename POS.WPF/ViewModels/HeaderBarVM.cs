using System.Windows.Input;

namespace POS.WPF.ViewModels
{
    public class HeaderBarVM
    {
        public string HeaderText { get; set; }
        public string IconKind { get; set; }
        public ICommand ButtonCommand { get; set; }
    }
}
