﻿using POS.DAL.Domain;
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

        public async Task Create(ProductDTM data)
        {
            var model = new Product();
            MapSingle(data, model);
            await dbContext.Products.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(ProductDTM data)
        {
            var model = await dbContext.Products.FindAsync(data.Id);
            MapSingle(data, model);
            await dbContext.SaveChangesAsync();
        }

        private void MapSingle(ProductDTM data, Product model)
        {
            model.Id = data.Id;
            model.Code = data.Code;
            model.CodeStatus = data.CodeStatus;
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
            if (data.Id == 0)
            {
                model.InsertedBy = data.InsertedBy;
                model.InsertedDate = data.InsertedDate;
            }
            else
            {
                model.UpdatedBy = data.UpdatedBy;
                model.UpdatedDate = data.UpdatedDate;
            }
            model.IsDeleted = data.IsDeleted;
        }

        public async Task<ProductDTM> GetById(int id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null) return null;
            return new ProductDTM
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

        public async Task<IEnumerable<ProductDTM>> GetList(int? categoryId)
        {
            var query = dbContext.Products.Where(p => p.IsDeleted == false);
            if (categoryId != null) query = query.Where(p => p.CategoryId == categoryId);
            return await query.Select(p => new ProductDTM
            {
                Id = p.Id,
                Code = p.Code,
                CodeStatus = p.CodeStatus,
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

        public async Task Delete(int[] ids)
        {
            var products = await dbContext.Products.Where(p => ids.Any(id => id == p.Id)).ToListAsync();
            dbContext.Products.RemoveRange(products);
            await dbContext.SaveChangesAsync();
        }
    }
}
