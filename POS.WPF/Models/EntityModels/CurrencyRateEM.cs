using MaterialDesignThemes.Wpf;
using POS.WPF.Validators.ModelValidators;
using System;

namespace POS.WPF.Models.EntityModels
{
    public class CurrencyRateEM : BaseErrorBindable<CurrencyRateEM>
    {
        public CurrencyRateEM() : base(new CurrencyRateValidator()) { }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetValue(ref _id, value); }
        }

        private int? _currencyId;
        public int? CurrencyId
        {
            get { return _currencyId; }
            set { SetAndValidate(ref _currencyId, value); }
        }

        private DateTime? _rateDate;
        public DateTime? RateDate
        {
            get { return _rateDate; }
            set { SetAndValidate(ref _rateDate, value); }
        }

        private string _baseValue = "1";
        public string BaseValue
        {
            get { return _baseValue; }
            set { SetAndValidate(ref _baseValue, value); OnPropertyChanged(nameof(FinalRate)); }
        }

        private string _rate;
        public string Rate
        {
            get { return _rate; }
            set { SetAndValidate(ref _rate, value); OnPropertyChanged(nameof(FinalRate)); }
        }

        private bool _reverseCalculation;
        public bool ReverseCalculation
        {
            get { return _reverseCalculation; }
            set { SetValue(ref _reverseCalculation, value); OnPropertyChanged(nameof(FinalRate)); }
        }

        public double? FinalRate
        {
            get
            {
                if (double.TryParse(Rate, out double _rate) && double.TryParse(BaseValue, out double _baseValue))
                {
                    if (_baseValue == 0) return null;
                    var _finalRate = _rate / _baseValue;
                    if (ReverseCalculation) _finalRate = 1d / _finalRate;
                    return Math.Round(_finalRate, 6);
                }
                return null;
            }
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

        public PackIconKind ReverseCalcIcon => ReverseCalculation ? PackIconKind.CheckBold : PackIconKind.MinusThick;

        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}
