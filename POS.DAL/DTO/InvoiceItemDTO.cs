namespace POS.DAL.DTO
{
    public class InvoiceItemDTO
    {
        public int SequenceNum { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    }
}
