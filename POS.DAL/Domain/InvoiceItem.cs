using System;
using System.Collections.Generic;

#nullable disable

namespace POS.DAL.Domain
{
    public partial class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int Price { get; set; }
        public int Cost { get; set; }
        public int Profit { get; set; }
        public int Discount { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Product Product { get; set; }
    }
}
