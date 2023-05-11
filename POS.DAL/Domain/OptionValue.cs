using System;
using System.Collections.Generic;

#nullable disable

namespace POS.DAL.Domain
{
    public partial class OptionValue
    {
        public OptionValue()
        {
            AccountAccountTypes = new HashSet<Account>();
            AccountCurrencies = new HashSet<Account>();
            CurrencyRates = new HashSet<CurrencyRate>();
            Invoices = new HashSet<Invoice>();
            ProductBrands = new HashSet<Product>();
            ProductCategories = new HashSet<Product>();
            ProductCurrencies = new HashSet<Product>();
            ProductUnits = new HashSet<Product>();
            Transactions = new HashSet<Transaction>();
            Treasuries = new HashSet<Treasury>();
        }

        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Flag { get; set; }
        public bool IsDefault { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsDeleted { get; set; }

        public virtual OptionType Type { get; set; }
        public virtual ICollection<Account> AccountAccountTypes { get; set; }
        public virtual ICollection<Account> AccountCurrencies { get; set; }
        public virtual ICollection<CurrencyRate> CurrencyRates { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Product> ProductBrands { get; set; }
        public virtual ICollection<Product> ProductCategories { get; set; }
        public virtual ICollection<Product> ProductCurrencies { get; set; }
        public virtual ICollection<Product> ProductUnits { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Treasury> Treasuries { get; set; }
    }
}
