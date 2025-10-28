using System.Collections.Generic;

namespace POS.DAL.DTO
{
    public class TotalsDTO
    {
        public int TotalSales { get; set; }
        public int TotalExpenses { get; set; }
        public int CurrentBalance { get; set; }

        public IEnumerable<BestProductDTO> BestProducts { get; set; }
        public IEnumerable<BestCustomerDTO> BestCustomers { get; set; }
    }

    public class BestProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantitySold { get; set; }
        public int TotalSales { get; set; }
        public int PercentageOfTotalSales { get; set; }
    }

    public class BestCustomerDTO
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public int TotalPurchases { get; set; }
        public int PercentageOfTotalPurchases { get; set; }
    }
}
