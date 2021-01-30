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
                    InitialQuantity = data.InitialQuantity,
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

        public async Task<ProductDT> GetById(int id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product != null)
            {
                return new ProductDT
                {
                    Id = product.Id,
                    Code = product.Code,
                    CodeStatus = product.CodeStatus,
                    Name = product.Name,
                    Cost = product.Cost,
                    Profit = product.Profit,
                    Price = product.Price,
                    Discount = product.Discount,
                    AlertQuantity = product.AlertQuantity,
                    InitialQuantity = product.InitialQuantity,
                    UnitId = product.UnitId,
                    BrandId = product.BrandId,
                    CategoryId = product.CategoryId,
                    CurrencyId = product.CurrencyId,
                    ExpiryDate = product.ExpiryDate,
                    Note = product.Note,
                    InsertedBy = product.InsertedBy,
                    InsertedDate = product.InsertedDate,
                    UpdatedBy = product.UpdatedBy,
                    UpdatedDate = product.UpdatedDate,
                    IsDeleted = product.IsDeleted,
                };
            }
            return null;
        }

        public async Task<IEnumerable<ProductDT>> GetList()
        {
            return await dbContext.Products
            .Where(x => x.IsDeleted == false)
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

        public async Task Delete(int[] ids)
        {
            var products = await dbContext.Products.Where(p => ids.Any(id => id == p.Id)).ToListAsync();
            dbContext.Products.RemoveRange(products);
            await dbContext.SaveChangesAsync();
        }
    }
}
