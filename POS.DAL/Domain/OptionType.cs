using System;
using System.Collections.Generic;

#nullable disable

namespace POS.DAL.Domain
{
    public partial class OptionType
    {
        public OptionType()
        {
            OptionValues = new HashSet<OptionValue>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<OptionValue> OptionValues { get; set; }
    }
}
