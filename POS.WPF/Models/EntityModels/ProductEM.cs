using POS.WPF.Enums;
using POS.WPF.Validators.ModelValidators;
using System;

namespace POS.WPF.Models.EntityModels
{
    public class ProductEM : BaseErrorBindable<ProductEM>
    {
        public ProductEM() : base(new ProductValidator()) { }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                OnPropertyChanged();
                ValidateField();
            }
        }

        private CodeStatus _codeStatus;
        public CodeStatus CodeStatus
        {
            get { return _codeStatus; }
            set
            {
                _codeStatus = value;
                OnPropertyChanged();
                ValidateField();
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
                ValidateField();
            }
        }

        private int? _initialQuantity;
        public int? InitialQuantity
        {
            get { return _initialQuantity; }
            set {
                _initialQuantity = value;
                OnPropertyChanged();
                ValidateField();
            }
        }

        private int? _cost;
        public int? Cost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Profit));
                ValidateField();
            }
        }

        private int? _price;
        public int? Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Profit));
                ValidateField();
            }
        }

        public int? Profit
        {
            get { return (Price ?? 0) - (Cost ?? 0); }
        }

        private int? _categoryId;
        public int? CategoryId
        {
            get { return _categoryId; }
            set
            {
                _categoryId = value;
                OnPropertyChanged();
                ValidateField();
            }
        }

        private int? _currencyId;
        public int? CurrencyId
        {
            get { return _currencyId; }
            set { _currencyId = value; OnPropertyChanged(); }
        }

        private int? _unitId;
        public int? UnitId
        {
            get { return _unitId; }
            set { _unitId = value; OnPropertyChanged(); }
        }

        private int? _brandId;
        public int? BrandId
        {
            get { return _brandId; }
            set { _brandId = value; OnPropertyChanged(); }
        }

        private int? _alertQuantity;
        public int? AlertQuantity
        {
            get { return _alertQuantity; }
            set { _alertQuantity = value; OnPropertyChanged(); }
        }

        private int? _discount;
        public int? Discount
        {
            get { return _discount; }
            set {
                _discount = value;
                OnPropertyChanged();
                ValidateField();
            }
        }

        private DateTime? _expiryDate;
        public DateTime? ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = value; OnPropertyChanged(); }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set { _note = value; OnPropertyChanged(); }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; OnPropertyChanged(); }
        }

        public string UnitName { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}
