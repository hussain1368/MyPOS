namespace POS.WPF.Enums
{
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

    public enum CalendarType
    {
        Gregorian = 1,
        HejriShamsi = 2,
    }
}
