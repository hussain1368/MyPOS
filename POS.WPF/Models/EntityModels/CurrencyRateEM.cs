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

        private int? _baseValue;
        public int? BaseValue
        {
            get { return _baseValue; }
            set { SetAndValidate(ref _baseValue, value); }
        }

        private double? _rate;
        public double? Rate
        {
            get { return _rate; }
            set { SetAndValidate(ref _rate, value); }
        }

        private bool _reverseCalculation;
        public bool ReverseCalculation
        {
            get { return _reverseCalculation; }
            set { SetValue(ref _reverseCalculation, value); }
        }

        private double? _finalRate;
        public double? FinalRate
        {
            get { return _finalRate; }
            set { SetAndValidate(ref _finalRate, value); }
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
    }
}
