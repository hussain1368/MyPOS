using System;
using System.Collections.Generic;

#nullable disable

namespace POS.DAL.Domain
{
    public partial class Product
    {
        public Product()
        {
            InvoiceItems = new HashSet<InvoiceItem>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Profit { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int InitialQuantity { get; set; }
        public int AlertQuantity { get; set; }
        public int? UnitId { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int CurrencyId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Note { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual OptionValue Brand { get; set; }
        public virtual OptionValue Category { get; set; }
        public virtual OptionValue Currency { get; set; }
        public virtual OptionValue Unit { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
