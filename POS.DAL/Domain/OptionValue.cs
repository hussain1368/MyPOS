using System;
using System.Collections.Generic;

#nullable disable

namespace POS.DAL.Domain
{
    public partial class OptionValue
    {
        public OptionValue()
        {
            Accounts = new HashSet<Account>();
            Invoices = new HashSet<Invoice>();
            ProductBrands = new HashSet<Product>();
            ProductCategories = new HashSet<Product>();
            ProductCurrencies = new HashSet<Product>();
            ProductUnits = new HashSet<Product>();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Flag { get; set; }
        public bool IsDeleted { get; set; }

        public virtual OptionType Type { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Product> ProductBrands { get; set; }
        public virtual ICollection<Product> ProductCategories { get; set; }
        public virtual ICollection<Product> ProductCurrencies { get; set; }
        public virtual ICollection<Product> ProductUnits { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
