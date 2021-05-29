using System;
using System.Collections.Generic;

#nullable disable

namespace POS.DAL.Domain
{
    public partial class Treasury
    {
        public Treasury()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }
        public int CurrentBalance { get; set; }
        public string Note { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }

        public virtual OptionValue Currency { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
