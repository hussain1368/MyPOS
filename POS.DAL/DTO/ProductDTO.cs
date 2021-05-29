using System;

namespace POS.DAL.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public byte CodeStatus { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Profit { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int AlertQuantity { get; set; }
        public int InitialQuantity { get; set; }
        public int? UnitId { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int CurrencyId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Note { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public string UnitName { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}
