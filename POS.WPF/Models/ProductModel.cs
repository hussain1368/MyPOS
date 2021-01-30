using POS.WPF.Enums;
using POS.WPF.ModelValidators;

namespace POS.WPF.Models
{
    public class ProductModel : BaseModelWithError<ProductModel>
    {
        public ProductModel() : base(new ProductValidator()) { }

        private int _id { get; set; }
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _code { get; set; }
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ValidateField();
                OnPropertyChanged();
            }
        }

        private CodeStatus _codeStatus { get; set; }
        public CodeStatus CodeStatus
        {
            get { return _codeStatus; }
            set { _codeStatus = value; OnPropertyChanged(); }
        }

        private string _name { get; set; }
        public string Name
        {
            get { return _name; }
            set { _name = value; ValidateField(); OnPropertyChanged(); }
        }

        private int? _initialQuantity { get; set; }
        public int? InitialQuantity
        {
            get { return _initialQuantity; }
            set { _initialQuantity = value; OnPropertyChanged(); }
        }

        private int? _cost { get; set; }
        public int? Cost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Profit));
            }
        }

        private int? _price { get; set; }
        public int? Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Profit));
            }
        }

        public int? Profit
        {
            get { return (Price ?? 0) - (Cost ?? 0); }
        }

        private int? _categoryId { get; set; }
        public int? CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; OnPropertyChanged(); }
        }

        public int Discount { get; set; }
        public string UnitName { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string CurrencyName { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; OnPropertyChanged(); }
        }
    }
}
