using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using POS.DAL.Repository.Abstraction;

namespace POS.DAL.Repository.DatabaseRepository
{
    public class OptionDatabaseRepository : BaseDatabaseRepository, IOptionRepository
    {
        public OptionDatabaseRepository(POSContext dbContext) : base(dbContext) { }

        public async Task<IList<OptionTypeDTO>> OptionTypes()
        {
            return await dbContext.OptionTypes.Select(t => new OptionTypeDTO
            {
                Id = t.Id,
                Code = t.Code,
                Name = t.Name,
                IsReadOnly = t.IsReadOnly,
                IsDeleted = t.IsDeleted,
            })
            .ToListAsync();
        }

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

        public async Task<IList<OptionValueDTO>> OptionsAll(bool includeDeleted = false)
        {
            var query = dbContext.OptionValues.AsQueryable();
            if (!includeDeleted)
                query = query.Where(v => v.IsDeleted == false);
            return await SelectOptions(query);
        }

        public async Task<IList<OptionValueDTO>> SelectOptions(IQueryable<OptionValue> query)
        {
            return await query.Select(v => new OptionValueDTO
            {
                Id = v.Id,
                TypeId = v.TypeId,
                TypeCode = v.Type.Code,
                Name = v.Name,
                Code = v.Code,
                IsDefault = v.IsDefault,
                IsReadOnly = v.IsReadOnly,
                IsDeleted = v.IsDeleted,
                Flag = v.Flag,
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

        public async Task<IEnumerable<WalletDTO>> GetWalletsList()
        {
            return await dbContext.Wallets.Where(t => !t.IsDeleted).Select(t => new WalletDTO
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

        //public async Task<SettingDTO> GetActiveSetting()
        //{
        //    var setting = await dbContext.Settings.FirstOrDefaultAsync(s => s.IsActive);
        //    return new SettingDTO
        //    {
        //        Id = setting.Id,
        //        LayoutName = setting.LayoutName,
        //        Language = setting.Language,
        //        AppTitle = setting.AppTitle,
        //        CalendarType = setting.CalendarType,
        //        IsActive = setting.IsActive,
        //    };
        //}

        public SettingDTO GetActiveSetting()
        {
            var setting = dbContext.Settings.FirstOrDefault(s => s.IsActive);
            return new SettingDTO
            {
                Id = setting.Id,
                LayoutName = setting.LayoutName,
                Language = setting.Language,
                AppTitle = setting.AppTitle,
                CalendarType = setting.CalendarType,
                IsActive = setting.IsActive,
            };
        }

        public async Task UpdateSetting(SettingDTO data)
        {
            var setting = await dbContext.Settings.FindAsync(data.Id);
            setting.Language = data.Language;
            setting.AppTitle = data.AppTitle;
            setting.CalendarType = data.CalendarType;
            await dbContext.SaveChangesAsync();
        }

        public async Task CreateOption(OptionValueDTO data)
        {
            await dbContext.OptionValues.AddAsync(new OptionValue
            {
                Code = data.Code,
                TypeId = data.TypeId,
                Name = data.Name,
            });
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateOption(OptionValueDTO data)
        {
            var option = await dbContext.OptionValues.FindAsync(data.Id);
            option.Code = data.Code;
            option.TypeId = data.TypeId;
            option.Name = data.Name;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteOptions(int[] ids)
        {
            var rows = await dbContext.OptionValues.Where(m => ids.Any(id => m.Id == id))
                .ExecuteUpdateAsync(m => m.SetProperty(m => m.IsDeleted, true));
        }

        public async Task RestoreOptions(int[] ids)
        {
            var rows = await dbContext.OptionValues.Where(m => ids.Any(id => m.Id == id))
                .ExecuteUpdateAsync(m => m.SetProperty(m => m.IsDeleted, false));
        }
    }
}
