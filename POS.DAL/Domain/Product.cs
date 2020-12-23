using System;
using System.Collections.Generic;

#nullable disable

namespace POS.DAL.Domain
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SalePrice { get; set; }
        public int? PurchasePrice { get; set; }
    }
}
