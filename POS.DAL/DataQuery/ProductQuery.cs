using POS.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POS.DAL.DataQuery
{
    public class ProductQuery
    {
        private readonly POSContext dbContext;

        public ProductQuery(POSContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateProduct()
        {
            await dbContext.Products.AddAsync(new Product
            {
                Name = "iPhone 12",
                SalePrice = 1200
            });
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetList()
        {
            return await dbContext.Products.ToListAsync();
        }
    }
}
