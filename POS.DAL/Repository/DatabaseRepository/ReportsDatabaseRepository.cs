using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.DTO;
using POS.DAL.Repository.Abstraction;
using System.Linq;
using System.Threading.Tasks;

namespace POS.DAL.Repository.DatabaseRepository
{
    public class ReportsDatabaseRepository : BaseDatabaseRepository, IReportsRepository
    {
        public ReportsDatabaseRepository(POSContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly POSContext _dbContext;

        public async Task<TotalsDTO> GetTotals()
        {
            var totalSales = await _dbContext.Invoices
                .Where(i => i.IsDeleted == false)
                .SumAsync(i => i.TotalPrice);

            var totalExpenses = await _dbContext.Transactions
                .Where(e => e.IsDeleted == false)
                .Where(e => e.TransactionType == 2)
                //.Where(e => e.SourceId != null)
                .SumAsync(e => e.Amount);

            var totalDeposit = await _dbContext.Transactions
                .Where(e => e.IsDeleted == false)
                .Where(e => e.TransactionType == 1)
                //.Where(e => e.SourceId != null)
                .SumAsync(e => e.Amount);

            var totals = new TotalsDTO
            {
                TotalSales = totalSales,
                TotalExpenses = totalExpenses,
                CurrentBalance = totalDeposit - totalExpenses
            };

            var query = _dbContext.InvoiceItems
                .Where(i => i.IsDeleted == false)
                .GroupBy(i => i.ProductId)
                .Select(g => new BestProductDTO
                {
                    ProductId = g.Key,
                    ProductName = g.FirstOrDefault().Product.Name,
                    TotalQuantitySold = g.Sum(i => i.Quantity),
                    TotalSales = g.Sum(i => i.TotalPrice),
                })
                .OrderByDescending(x => x.TotalQuantitySold)
                .Take(5);

            var sql = query.ToQueryString();

            var data = await query.ToListAsync();

            data = data.Select(d =>
                {
                    d.PercentageOfTotalSales = totalSales > 0 ? (int)(d.TotalSales / (double)totalSales * 100) : 0;
                    return d;
                })
                .ToList();

            var query2 = _dbContext.Invoices
                .Where(i => i.IsDeleted == false && i.PartnerId != null)
                .GroupBy(i => i.PartnerId)
                .Select(g => new BestCustomerDTO
                {
                    PartnerId = g.Key.Value,
                    PartnerName = g.FirstOrDefault().Partner.Name,
                    TotalPurchases = g.Sum(i => i.TotalPrice)
                })
                .OrderByDescending(x => x.TotalPurchases)
                .Take(5);

            var data2 = await query2.ToListAsync();

            data2 = data2.Select(d =>
                {
                    d.PercentageOfTotalPurchases = totalSales > 0 ? (int)(d.TotalPurchases / (double)totalSales * 100) : 0;
                    return d;
                })
                .ToList();

            totals.BestProducts = data;
            totals.BestCustomers = data2;

            return totals;
        }
    }
}