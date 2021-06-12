using System;
using POS.WPF.Validators.ModelValidators;

namespace POS.WPF.Models.EntityModels
{
    public class ProductEM : BaseErrorBindable<ProductEM>
    {
        public ProductEM() : base(new ProductValidator()) { }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetValue(ref _id, value); }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set { SetAndValidate(ref _code, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetAndValidate(ref _name, value); }
        }

        private int? _initialQuantity;
        public int? InitialQuantity
        {
            get { return _initialQuantity; }
            set { SetAndValidate(ref _initialQuantity, value); }
        }

        private int? _cost;
        public int? Cost
        {
            get { return _cost; }
            set
            {
                SetAndValidate(ref _cost, value);
                OnPropertyChanged(nameof(Profit));
            }
        }

        private int? _price;
        public int? Price
        {
            get { return _price; }
            set
            {
                SetAndValidate(ref _price, value);
                OnPropertyChanged(nameof(Profit));
            }
        }

        public int? Profit
        {
            get
            {
                var _value = (Price ?? 0) - (Cost ?? 0);
                if (_value == 0) return null;
                return _value;
            }
        }

        private int? _categoryId;
        public int? CategoryId
        {
            get { return _categoryId; }
            set { SetAndValidate(ref _categoryId, value); }
        }

        private int? _currencyId;
        public int? CurrencyId
        {
            get { return _currencyId; }
            set { SetValue(ref _currencyId, value); }
        }

        private int? _unitId;
        public int? UnitId
        {
            get { return _unitId; }
            set { SetValue(ref _unitId, value); }
        }

        private int? _brandId;
        public int? BrandId
        {
            get { return _brandId; }
            set { SetValue(ref _brandId, value); }
        }

        private int? _alertQuantity;
        public int? AlertQuantity
        {
            get { if (_alertQuantity == 0) return null; return _alertQuantity; }
            set { SetValue(ref _alertQuantity, value); }
        }

        private int? _discount;
        public int? Discount
        {
            get { if (_discount == 0) return null; return _discount; }
            set { SetAndValidate(ref _discount, value); }
        }

        private DateTime? _expiryDate;
        public DateTime? ExpiryDate
        {
            get { return _expiryDate; }
            set { SetValue(ref _expiryDate, value); }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set { SetValue(ref _note, value); }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetValue(ref _isChecked, value); }
        }

        public string UnitName { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}
