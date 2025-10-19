using System;
using System.Collections.Generic;

namespace POS.DAL.Domain;

public partial class Invoice
{
    public int Id { get; set; }

    public string SerialNum { get; set; }

    public byte InvoiceType { get; set; }

    public int WarehouseId { get; set; }

    public int WalletId { get; set; }

    public int? PartnerId { get; set; }

    public string PartnerName { get; set; }

    public int CurrencyId { get; set; }

    public double CurrencyRate { get; set; }

    public DateTime IssueDate { get; set; }

    public byte PaymentType { get; set; }

    public double AmountPaid { get; set; }

    public double OverallDiscount { get; set; }

    public int TotalPrice { get; set; }

    public int ItemsCount { get; set; }

    public string Note { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Partner Partner { get; set; }

    public virtual OptionValue Currency { get; set; }

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual Wallet Wallet { get; set; }

    public virtual AppUser UpdatedByNavigation { get; set; }

    public virtual Warehouse Warehouse { get; set; }
}
