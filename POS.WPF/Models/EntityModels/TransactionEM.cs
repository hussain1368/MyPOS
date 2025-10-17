using POS.WPF.Enums;
using POS.WPF.Validators.ModelValidators;
using System;

namespace POS.WPF.Models.EntityModels
{
    public class TransactionEM : BaseErrorBindable<TransactionEM>
    {
        public TransactionEM() : base(new TransactionValidator()) { }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _amount;
        public string Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged(); ValidateField(); }
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); ValidateField(); }
        }

        private int? _partnerId;
        public int? PartnerId
        {
            get { return _partnerId; }
            set { _partnerId = value; OnPropertyChanged(); }
        }

        private string _partnerName;
        public string PartnerName
        {
            get { return _partnerName; }
            set { _partnerName = value; OnPropertyChanged(); ValidateField(); }
        }

        private int? _currencyId;
        public int? CurrencyId
        {
            get { return _currencyId; }
            set { _currencyId = value; OnPropertyChanged(); ValidateField(); }
        }

        private string _currencyRate;
        public string CurrencyRate
        {
            get { return _currencyRate; }
            set { _currencyRate = value; OnPropertyChanged(); ValidateField(); }
        }

        private TransactionType _transactionType;
        public TransactionType TransactionType
        {
            get { return _transactionType; }
            set { _transactionType = value; OnPropertyChanged(); ValidateField(); }
        }

        private int? _sourceId;
        public int? SourceId
        {
            get { return _sourceId; }
            set { _sourceId = value; OnPropertyChanged(); ValidateField(); }
        }

        private int? _walletId;
        public int? WalletId
        {
            get { return _walletId; }
            set { _walletId = value; OnPropertyChanged(); ValidateField(); }
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

        private bool _walletNameReadOnly;
        public bool PartnerNameReadOnly
        {
            get { return _walletNameReadOnly; }
            set { _walletNameReadOnly = value; OnPropertyChanged(); }
        }

        public string WalletName { get; set; }
        public string SourceName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}
