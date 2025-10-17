using System;

namespace POS.DAL.DTO
{
    public class PartnerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public int CurrencyId { get; set; }
        public int CurrentBalance { get; set; }
        public int PartnerTypeId { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public string PartnerTypeName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}
