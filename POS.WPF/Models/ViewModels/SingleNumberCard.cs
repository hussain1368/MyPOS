using MaterialDesignThemes.Wpf;

namespace POS.WPF.Models.ViewModels
{
    public class SingleNumberCard : BaseBindable
    {
        public string Title { get; set; }
        public PackIconKind IconKind { get; set; }
        public double Number { get; set; }
    }
}
