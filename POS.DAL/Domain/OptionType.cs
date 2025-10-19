using System.Collections.Generic;

namespace POS.DAL.Domain;

public partial class OptionType
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public bool IsReadOnly { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<OptionValue> OptionValues { get; set; } = new List<OptionValue>();
}
