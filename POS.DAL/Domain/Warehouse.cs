using System;
using System.Collections.Generic;

#nullable disable

namespace POS.DAL.Domain
{
    public partial class Warehouse
    {
        public Warehouse()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
