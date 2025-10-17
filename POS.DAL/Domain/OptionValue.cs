using System;
using System.Collections.Generic;

namespace POS.DAL.Domain;

public partial class OptionValue
{
    public int Id { get; set; }

    public int TypeId { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string Flag { get; set; }

    public bool IsDefault { get; set; }

    public bool IsReadOnly { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Account> AccountAccountTypes { get; set; } = new List<Account>();

    public virtual ICollection<Account> AccountCurrencies { get; set; } = new List<Account>();

    public virtual ICollection<CurrencyRate> CurrencyRates { get; set; } = new List<CurrencyRate>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Product> ProductBrands { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductCategories { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductCurrencies { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductUnits { get; set; } = new List<Product>();

    public virtual ICollection<Transaction> TransactionCurrencies { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionSources { get; set; } = new List<Transaction>();

    public virtual ICollection<Treasury> Treasuries { get; set; } = new List<Treasury>();

    public virtual OptionType Type { get; set; }
}
