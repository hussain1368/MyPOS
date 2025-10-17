using POS.WPF.Validators.ModelValidators;

namespace POS.WPF.Models.EntityModels
{
    public class PartnerEM : BaseErrorBindable<PartnerEM>
    {
        public PartnerEM() : base(new PartnerValidator()) { }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); ValidateField(); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged(); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(); }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set { _note = value; OnPropertyChanged(); }
        }

        private int? _currencyId;
        public int? CurrencyId
        {
            get { return _currencyId; }
            set { _currencyId = value; OnPropertyChanged(); ValidateField(); }
        }

        private int? _partnerTypeId;
        public int? PartnerTypeId
        {
            get { return _partnerTypeId; }
            set { _partnerTypeId = value; OnPropertyChanged(); ValidateField(); }
        }

        private int _currentBalance;
        public int CurrentBalance
        {
            get { return _currentBalance; }
            set { _currentBalance = value; OnPropertyChanged(); }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; OnPropertyChanged(); }
        }

        public string PartnerTypeName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}
