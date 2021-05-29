namespace POS.WPF.Models.EntityModels
{
    public class InvoiceItemEM : BaseBindable
    {
        private int _index;
        public int Index
        {
            get => _index;
            set => SetValue(ref _index, value);
        }

        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int UnitDiscount { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                SetValue(ref _quantity, value);
                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(TotalDiscount));
            }
        }
        
        public int TotalPrice => (UnitPrice - UnitDiscount) * Quantity;
        public int TotalDiscount => UnitDiscount * Quantity;
    }
}
