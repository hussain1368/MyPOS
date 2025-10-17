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

        private string _initialQuantity;
        public string InitialQuantity
        {
            get { return _initialQuantity; }
            set { SetAndValidate(ref _initialQuantity, value); }
        }

        private string _cost;
        public string Cost
        {
            get { return _cost; }
            set
            {
                SetAndValidate(ref _cost, value);
                OnPropertyChanged(nameof(Profit));
            }
        }

        private string _price;
        public string Price
        {
            get { return _price; }
            set
            {
                SetAndValidate(ref _price, value);
                OnPropertyChanged(nameof(Profit));
            }
        }

        public string Profit
        {
            get
            {
                if (int.TryParse(Cost, out var _cost) && int.TryParse(Price, out var _price))
                {
                    var _value = _price - _cost;
                    if (_value == 0) return null;
                    return _value.ToString();
                }
                return null;
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

        private string _alertQuantity;
        public string AlertQuantity
        {
            get
            {
                if (int.TryParse(_alertQuantity, out int _val) && _val == 0) return null; 
                return _alertQuantity;
            }
            set { SetAndValidate(ref _alertQuantity, value); }
        }

        private string _discount;
        public string Discount
        {
            get
            {
                if (int.TryParse(_discount, out int _val) && _val == 0) return null;
                return _discount;
            }
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
