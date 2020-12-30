using System;
using System.Collections.Generic;

#nullable disable

namespace POS.DAL.Domain
{
    public partial class Account
    {
        public Account()
        {
            Invoices = new HashSet<Invoice>();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public int CurrencyId { get; set; }
        public int CurrentBalance { get; set; }
        public int AccountType { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual OptionValue Currency { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
