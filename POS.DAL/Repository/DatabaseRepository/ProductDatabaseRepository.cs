using POS.DAL.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.DAL.DTO;
using System.Linq;
using POS.DAL.Repository.Abstraction;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace POS.DAL.Repository.DatabaseRepository
{
    public class ProductDatabaseRepository : BaseDatabaseRepository, IProductRepository
    {
        private readonly IMapper mapper;

        public ProductDatabaseRepository(POSContext dbContext, IMapper mapper) : base(dbContext)
        {
            this.mapper = mapper;
        }

        public async Task Create(ProductDTO data)
        {
            var model = mapper.Map<Product>(data);
            await dbContext.Products.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(ProductDTO data)
        {
            var model = await dbContext.Products.FindAsync(data.Id);
            mapper.Map(data, model);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ProductDTO> GetById(int id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null) return null;
            return mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetList(int? categoryId)
        {
            var query = dbContext.Products.Where(p => p.IsDeleted == false);
            if (categoryId != null) query = query.Where(p => p.CategoryId == categoryId);

            return await query.ProjectTo<ProductDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<ProductItemDTO> GetByCode(string code)
        {
            var product = await dbContext.Products.SingleOrDefaultAsync(p => p.Code == code && !p.IsDeleted);
            if (product == null) return null;
            return mapper.Map<ProductItemDTO>(product);
        }

        public async Task<IEnumerable<ProductItemDTO>> GetByName(string searchValue)
        {
            var query = dbContext.Products
                .Where(p => p.IsDeleted == false)
                .Where(p => p.Name.Contains(searchValue));
            return await query
                .ProjectTo<ProductItemDTO>(mapper.ConfigurationProvider)
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
