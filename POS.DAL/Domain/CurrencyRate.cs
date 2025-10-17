using System;
using System.Collections.Generic;

namespace POS.DAL.Domain;

public partial class CurrencyRate
{
    public int Id { get; set; }

    public int CurrencyId { get; set; }

    public DateTime RateDate { get; set; }

    public int BaseValue { get; set; }

    public double Rate { get; set; }

    public double FinalRate { get; set; }

    public bool ReverseCalculation { get; set; }

    public string Note { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    public virtual OptionValue Currency { get; set; }
}
