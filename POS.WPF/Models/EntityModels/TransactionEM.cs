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

        private int _amount;
        public int Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged(); ValidateField(); }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); ValidateField(); }
        }

        private int? _accountId;
        public int? AccountId
        {
            get { return _accountId; }
            set { _accountId = value; OnPropertyChanged(); ValidateField(); }
        }

        private int? _invoiceId;
        public int? InvoiceId
        {
            get { return _invoiceId; }
            set { _invoiceId = value; OnPropertyChanged(); ValidateField(); }
        }

        private byte _transactionType;
        public byte TransactionType
        {
            get { return _transactionType; }
            set { _transactionType = value; OnPropertyChanged(); }
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

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; OnPropertyChanged(); }
        }

        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}
