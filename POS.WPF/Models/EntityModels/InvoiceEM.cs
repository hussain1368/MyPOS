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

        private TreasuryDTO _treasury;
        public TreasuryDTO Treasury
        {
            get => _treasury;
            set => SetAndValidate(ref _treasury, value);
        }

        private int? _warehouseId;
        public int? WarehouseId
        {
            get => _warehouseId;
            set => SetAndValidate(ref _warehouseId, value);
        }

        private int? _accountId;
        public int? AccountId
        {
            get => _accountId;
            set => SetValue(ref _accountId, value);
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
    }
}
