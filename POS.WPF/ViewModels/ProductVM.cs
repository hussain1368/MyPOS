namespace POS.WPF.ViewModels
{
    public class ProductVM : BaseVM
    {
        private long id;
        private string name;
        private long? salePrice;

        public long Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        public long? SalePrice
        {
            get { return salePrice; }
            set { salePrice = value; OnPropertyChanged(nameof(SalePrice)); }
        }
    }
}
