namespace POS.WPF.Enums
{
    public enum CodeStatus
    {
        HasCode = 1,
        NewCode = 2,
        NoCode = 3,
    }

    public enum InvoiceType
    {
        Sale = 1,
        Purchase = 2,
        Return = 3,
    }

    public enum PaymentType
    {
        None = 0,
        Cash = 1,
        Loan = 2,
    }
}
