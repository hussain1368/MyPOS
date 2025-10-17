using System;

namespace POS.DAL.DTO
{
    public class InvoiceRowDTO
    {
        public int Id { get; set; }
        public string SerialNum { get; set; }
        public byte InvoiceType { get; set; }
        public int WarehouseId { get; set; }
        public int WalletId { get; set; }
        public int? PartnerId { get; set; }
        public int CurrencyId { get; set; }
        public double CurrencyRate { get; set; }
        public DateTime IssueDate { get; set; }
        public byte? PaymentType { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int ItemsCount { get; set; }
        public int TotalPrice { get; set; }

        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public string PartnerName { get; set; }
        public string WalletName { get; set; }
    }
}
