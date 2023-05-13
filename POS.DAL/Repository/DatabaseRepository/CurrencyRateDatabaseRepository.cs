using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.DTO;
using POS.DAL.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.DAL.Repository.DatabaseRepository
{
    public class CurrencyRateDatabaseRepository : BaseDatabaseRepository, ICurrencyRateRepository
    {
        public CurrencyRateDatabaseRepository(POSContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<CurrencyRateDTO>> GetList(int? currencyId)
        {
            var query = dbContext.CurrencyRates.Where(r => r.IsDeleted == false);
            if (currencyId != null)
            {
                query = query.Where(r => r.CurrencyId == currencyId);
            }
            return await query.Select(r => new CurrencyRateDTO
            {
                Id = r.Id,
                CurrencyId = r.CurrencyId,
                CurrencyName = r.Currency.Name,
                RateDate = r.RateDate,
                BaseValue = r.BaseValue,
                Rate = r.Rate,
                ReverseCalculation = r.ReverseCalculation,
                FinalRate = r.FinalRate,
                Note = r.Note,
                IsDeleted = r.IsDeleted,
            })
            .ToListAsync();
        }

        public async Task Create(CurrencyRateDTO data)
        {
            var model = new CurrencyRate();
            MapSingle(data, model);
            await dbContext.CurrencyRates.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        private void MapSingle(CurrencyRateDTO data, CurrencyRate model)
        {
            model.Id = data.Id;
            model.CurrencyId = data.CurrencyId;
            model.RateDate = data.RateDate;
            model.BaseValue = data.BaseValue;
            model.Rate = data.Rate;
            model.ReverseCalculation = data.ReverseCalculation;
            model.FinalRate = data.FinalRate;
            model.Note = data.Note;
            model.IsDeleted = data.IsDeleted;
            model.UpdatedBy = data.UpdatedBy;
            model.UpdatedDate = data.UpdatedDate;
        }

        public async Task Update(CurrencyRateDTO data)
        {
            var model = await dbContext.CurrencyRates.FindAsync(data.Id);
            MapSingle(data, model);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(int[] ids)
        {
            var rows = await dbContext.CurrencyRates.Where(r => ids.Contains(r.Id)).ToListAsync();
            foreach (var row in rows) row.IsDeleted = true;
            await dbContext.SaveChangesAsync();
        }
    }
}
