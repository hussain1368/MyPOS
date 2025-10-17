using System;
using System.Collections.Generic;

namespace POS.DAL.Domain;

public partial class InvoiceItem
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }

    public int ProductId { get; set; }

    public int UnitPrice { get; set; }

    public int TotalPrice { get; set; }

    public int Cost { get; set; }

    public int Profit { get; set; }

    public int UnitDiscount { get; set; }

    public int TotalDiscount { get; set; }

    public int Quantity { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Invoice Invoice { get; set; }

    public virtual Product Product { get; set; }
}
