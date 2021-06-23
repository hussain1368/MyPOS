using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace POS.DAL.Repository
{
    public class OptionRepository : BaseRepository
    {
        public OptionRepository(POSContext dbContext): base(dbContext) { }

        public async Task<IList<OptionValueDTO>> OptionsByTypeId(int typeId)
        {
            var query = dbContext.OptionValues.Where(v => v.IsDeleted == false).Where(x => x.TypeId == typeId);
            return await SelectOptions(query);
        }

        public async Task<IList<OptionValueDTO>> OptionsByTypeCode(string typeCode)
        {
            var query = dbContext.OptionValues.Where(v => v.IsDeleted == false).Where(x => x.Type.Code == typeCode);
            return await SelectOptions(query);
        }

        public async Task<IList<OptionValueDTO>> OptionsAll()
        {
            var query = dbContext.OptionValues.Where(v => v.IsDeleted == false);
            return await SelectOptions(query);
        }

        private async Task<IList<OptionValueDTO>> SelectOptions(IQueryable<OptionValue> query)
        {
            return await query.Select(v => new OptionValueDTO
            {
                Id = v.Id,
                TypeId = v.TypeId,
                TypeCode = v.Type.Code,
                Name = v.Name,
                Code = v.Code,
                IsDefault = v.IsDefault,
            })
            .ToListAsync();
        }

        public async Task<OptionValueDTO> OptionByCode(string code)
        {
            return await dbContext.OptionValues.Where(x => x.Code == code).Select(v => new OptionValueDTO
            {
                Id = v.Id,
                TypeId = v.TypeId,
                TypeCode = v.Type.Code,
                Name = v.Name,
                Code = v.Code
            })
            .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<TreasuryDTO>> GetTreasuriesList()
        {
            return await dbContext.Treasuries.Where(t => !t.IsDeleted).Select(t => new TreasuryDTO
            {
                Id = t.Id,
                Name = t.Name,
                CurrencyId = t.CurrencyId,
                CurrentBalance = t.CurrentBalance,
                Note = t.Note,
                CurrencyName = t.Currency.Name,
                CurrencyCode = t.Currency.Code,
                IsDefault = t.IsDefault,
            })
            .ToListAsync();
        }

        public async Task<IEnumerable<WarehouseDTO>> GetWarehousesList()
        {
            return await dbContext.Warehouses.Where(w => !w.IsDeleted).Select(w => new WarehouseDTO
            {
                Id = w.Id,
                Name = w.Name,
                Note = w.Note,
                IsDefault = w.IsDefault,
            })
            .ToListAsync();
        }
    }
}
