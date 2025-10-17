using POS.DAL.DTO;
using POS.WPF.Enums;
using POS.WPF.Validators.ModelValidators;
using System;

namespace POS.WPF.Models.EntityModels
{
    public class InvoiceEM :BaseErrorBindable<InvoiceEM>
    {
        public InvoiceEM() : base(new InvoiceValidator()) { }

        private int _id;
        public int Id
        {
            get => _id;
            set => SetValue(ref _id, value);
        }

        private WalletDTO _wallet;
        public WalletDTO Wallet
        {
            get => _wallet;
            set => SetAndValidate(ref _wallet, value);
        }

        private int? _warehouseId;
        public int? WarehouseId
        {
            get => _warehouseId;
            set => SetAndValidate(ref _warehouseId, value);
        }

        private int? _partnerId;
        public int? PartnerId
        {
            get => _partnerId;
            set => SetValue(ref _partnerId, value);
        }

        private DateTime? _issueDate = DateTime.Now;
        public DateTime? IssueDate
        {
            get => _issueDate;
            set => SetAndValidate(ref _issueDate, value);
        }

        private double? _currencyRate = 1;
        public double? CurrencyRate
        {
            get => _currencyRate;
            set => SetAndValidate(ref _currencyRate, value);
        }

        private PaymentType _paymentType;
        public PaymentType PaymentType
        {
            get => _paymentType;
            set => SetAndValidate(ref _paymentType, value);
        }

        private string _note;
        public string Note
        {
            get => _note;
            set => SetValue(ref _note, value);
        }

        public string WalletName { get; set; }
    }
}
