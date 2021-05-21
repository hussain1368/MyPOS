namespace POS.WPF.Models.EntityModels
{
    public class InvoiceItemEM : BaseBindable
    {
        public int SequenceNum { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Discount { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                SetValue(ref _quantity, value);
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
        
        public int TotalPrice => (UnitPrice - Discount) * Quantity;
    }
}
