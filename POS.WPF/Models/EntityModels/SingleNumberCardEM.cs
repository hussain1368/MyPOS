using MaterialDesignThemes.Wpf;

namespace POS.WPF.Models.EntityModels
{
    public class SingleNumberCardEM : BaseBindable
    {
        public string Title { get; set; }
        public PackIconKind IconKind { get; set; }

        private double _number;
        public double Number
        {
            get => _number;
            set => SetValue(ref _number, value);
        }
    }
}
