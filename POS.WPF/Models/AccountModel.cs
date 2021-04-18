using POS.WPF.ModelValidators;

namespace POS.WPF.Models
{
    public class AccountModel : BaseErrorBindable<AccountModel>
    {
        public AccountModel() : base(new AccountValidator()) { }

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

        private int? _accountTypeId;
        public int? AccountTypeId
        {
            get { return _accountTypeId; }
            set { _accountTypeId = value; OnPropertyChanged(); ValidateField(); }
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

        public string AccountTypeName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}
