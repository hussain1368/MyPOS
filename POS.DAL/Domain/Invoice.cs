using System;
using System.Collections.Generic;

#nullable disable

namespace POS.DAL.Domain
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string SerialNum { get; set; }
        public int? WarehouseId { get; set; }
        public int? AccountId { get; set; }
        public string AccountName { get; set; }
        public DateTime IssueDate { get; set; }
        public string Note { get; set; }
        public int CurrencyId { get; set; }
        public int CurrencyRate { get; set; }
        public byte? PaymentType { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Account Account { get; set; }
        public virtual OptionValue Currency { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
