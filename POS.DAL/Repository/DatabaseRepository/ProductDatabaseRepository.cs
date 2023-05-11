using POS.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.DAL.DTO;
using System.Linq;
using System;
using POS.DAL.Repository.Abstraction;

namespace POS.DAL.Repository.DatabaseRepository
{
    public class ProductDatabaseRepository : BaseDatabaseRepository, IProductRepository
    {
        public ProductDatabaseRepository(POSContext dbContext) : base(dbContext)
        {
        }

        public async Task Create(ProductDTO data)
        {
            var model = new Product();
            MapSingle(data, model);
            await dbContext.Products.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(ProductDTO data)
        {
            var model = await dbContext.Products.FindAsync(data.Id);
            MapSingle(data, model);
            await dbContext.SaveChangesAsync();
        }

        private void MapSingle(ProductDTO data, Product model)
        {
            model.Id = data.Id;
            model.Name = data.Name;
            model.Cost = data.Cost;
            model.Profit = data.Profit;
            model.Price = data.Price;
            model.Discount = data.Discount;
            model.AlertQuantity = data.AlertQuantity;
            model.InitialQuantity = data.InitialQuantity;
            model.UnitId = data.UnitId;
            model.BrandId = data.BrandId;
            model.CategoryId = data.CategoryId;
            model.CurrencyId = data.CurrencyId;
            model.ExpiryDate = data.ExpiryDate;
            model.Note = data.Note;
            model.UpdatedBy = data.UpdatedBy;
            model.UpdatedDate = data.UpdatedDate;
            model.IsDeleted = data.IsDeleted;

            if (string.IsNullOrWhiteSpace(data.Code))
                model.Code = $"{DateTime.Now.DayOfYear}{new Random().Next(100_000_000, 900_000_000)}";
            else
                model.Code = data.Code;
        }

        public async Task<ProductDTO> GetById(int id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null) return null;
            return new ProductDTO
            {
                Id = product.Id,
                Code = product.Code,
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
                UpdatedBy = product.UpdatedBy,
                UpdatedDate = product.UpdatedDate,
                IsDeleted = product.IsDeleted,
            };
        }

        public async Task<IEnumerable<ProductDTO>> GetList(int? categoryId)
        {
            var query = dbContext.Products.Where(p => p.IsDeleted == false);
            if (categoryId != null) query = query.Where(p => p.CategoryId == categoryId);
            return await query.Select(p => new ProductDTO
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Cost = p.Cost,
                Profit = p.Profit,
                Price = p.Price,
                Discount = p.Discount,
                AlertQuantity = p.AlertQuantity,
                UnitId = p.UnitId,
                BrandId = p.BrandId,
                CategoryId = p.CategoryId,
                CurrencyId = p.CurrencyId,
                ExpiryDate = p.ExpiryDate,
                Note = p.Note,
                UnitName = p.UnitId != null ? p.Unit.Name : null,
                BrandName = p.BrandId != null ? p.Brand.Name : null,
                CategoryName = p.CategoryId != null ? p.Category.Name : null,
                CurrencyName = p.Currency.Name,
                CurrencyCode = p.Currency.Code,
            })
            .ToListAsync();
        }

        public async Task<ProductItemDTO> GetByCode(string code)
        {
            var product = await dbContext.Products.SingleOrDefaultAsync(p => p.Code == code && !p.IsDeleted);
            if (product == null) return null;
            return new ProductItemDTO
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Price = product.Price,
                Discount = product.Discount,
            };
        }

        public async Task<IEnumerable<ProductItemDTO>> GetByName(string searchValue)
        {
            var query = dbContext.Products
                .Where(p => p.IsDeleted == false)
                .Where(p => p.Name.Contains(searchValue));
            return await query.Select(p => new ProductItemDTO
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Price = p.Price,
                Discount = p.Discount,
            })
            .Take(5).ToListAsync();
        }

        public async Task Delete(int[] ids)
        {
            var products = await dbContext.Products.Where(p => ids.Any(id => id == p.Id)).ToListAsync();
            foreach (var product in products) product.IsDeleted = true;
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckDuplicate(int id, string code)
        {
            return await dbContext.Products.AnyAsync(p => p.Id != id && p.Code == code);
        }
    }
}
