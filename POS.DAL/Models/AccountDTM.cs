using System;

namespace POS.DAL.Models
{
    public class AccountDTM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public int CurrencyId { get; set; }
        public int CurrentBalance { get; set; }
        public int AccountType { get; set; }
        public int InsertedBy { get; set; }
        public DateTime InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}
