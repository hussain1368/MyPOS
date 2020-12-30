using System;
using System.Collections.Generic;

#nullable disable

namespace POS.DAL.Domain
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public int? AccountId { get; set; }
        public string AccountName { get; set; }
        public int? InvoiceId { get; set; }
        public byte TransactionType { get; set; }
        public int CurrencyId { get; set; }
        public int CurrencyRate { get; set; }
        public string Note { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InserteDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Account Account { get; set; }
        public virtual OptionValue Currency { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
