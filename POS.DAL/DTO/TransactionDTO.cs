using POS.DAL.Domain;
using System;

namespace POS.DAL.DTO
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public int TreasuryId { get; set; }
        public int? AccountId { get; set; }
        public string AccountName { get; set; }
        public int? InvoiceId { get; set; }
        public byte TransactionType { get; set; }
        public int SourceId { get; set; }
        public int CurrencyId { get; set; }
        public double CurrencyRate { get; set; }
        public string Note { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public string TreasuryName { get; set; }
        public string SourceName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}
