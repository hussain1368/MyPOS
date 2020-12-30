using POS.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.DAL.Models;
using System.Linq;

namespace POS.DAL.Query
{
    public class ProductQuery : DbQuery
    {
        public ProductQuery(POSContext dbContext) : base(dbContext)
        {
        }

        public async Task Create(ProductDT data)
        {
            try
            {
                var model = new Product
                {
                    Code = data.Code,
                    CodeStatus = data.CodeStatus,
                    Name = data.Name,
                    Cost = data.Cost,
                    Profit = data.Profit,
                    Price = data.Price,
                    Discount = data.Discount,
                    AlertQuantity = data.AlertQuantity,
                    UnitId = data.UnitId,
                    BrandId = data.BrandId,
                    CategoryId = data.CategoryId,
                    CurrencyId = data.CurrencyId,
                    ExpiryDate = data.ExpiryDate,
                    Note = data.Note,
                    InsertedBy = data.InsertedBy,
                    InsertedDate = data.InsertedDate,
                    UpdatedBy = data.UpdatedBy,
                    UpdatedDate = data.UpdatedDate,
                    IsDeleted = data.IsDeleted,
                };
                await dbContext.Products.AddAsync(model);
                await dbContext.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public async Task<IEnumerable<ProductDT>> GetList()
        {
            return await dbContext.Products
            .Where(x=>x.IsDeleted==false)
            .Select(x => new ProductDT
            {
                Id = x.Id,
                Code = x.Code,
                CodeStatus = x.CodeStatus,
                Name = x.Name,
                Cost = x.Cost,
                Profit = x.Profit,
                Price = x.Price,
                Discount = x.Discount,
                AlertQuantity = x.AlertQuantity,
                UnitId = x.UnitId,
                BrandId = x.BrandId,
                CategoryId = x.CategoryId,
                CurrencyId = x.CurrencyId,
                ExpiryDate = x.ExpiryDate,
                Note = x.Note,
                UnitName = x.UnitId != null ? x.Unit.Name : null,
                BrandName = x.BrandId != null ? x.Brand.Name : null,
                CategoryName = x.CategoryId != null ? x.Category.Name : null,
                CurrencyName = x.Currency.Name,
            })
            .ToListAsync();
        }
    }
}
