namespace POS.DAL.DTO
{
    public class InvoiceItemDTO
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int UnitPrice { get; set; }
        public int TotalPrice { get; set; }
        public int Cost { get; set; }
        public int Profit { get; set; }
        public int UnitDiscount { get; set; }
        public int TotalDiscount { get; set; }
        public int Quantity { get; set; }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
    }
}
