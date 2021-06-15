using MaterialDesignThemes.Wpf;

namespace POS.WPF.Models.EntityModels
{
    public class MenuItemEM
    {
        public BaseBindable ViewModel { get; set; }
        public PackIconKind IconKind { get; set; }
        public string Text { get; set; }
    }
}
