using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        public CurrencyRateDatabaseRepository(POSContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        private readonly IMapper _mapper;

        public async Task<IEnumerable<CurrencyRateDTO>> GetList(int? currencyId, DateTime? date)
        {
            var query = dbContext.CurrencyRates.Where(r => r.IsDeleted == false);

            if (currencyId != null) query = query.Where(r => r.CurrencyId == currencyId);
            if (date != null) query = query.Where(r => r.RateDate.Date == date.Value.Date);

            return await query
                .ProjectTo<CurrencyRateDTO>(_mapper.ConfigurationProvider)
                .OrderByDescending(r => r.RateDate)
                .ToListAsync();
        }

        public async Task Create(CurrencyRateDTO data)
        {
            var model = _mapper.Map<CurrencyRate>(data);
            await dbContext.CurrencyRates.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(CurrencyRateDTO data)
        {
            var model = await dbContext.CurrencyRates.FindAsync(data.Id);
            _mapper.Map(data, model);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(int[] ids)
        {
            await dbContext.CurrencyRates.Where(r => ids.Contains(r.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(r => r.IsDeleted, r => true));
        }
    }
}
