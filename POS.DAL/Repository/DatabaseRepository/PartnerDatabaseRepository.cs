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
    public class PartnerDatabaseRepository : BaseDatabaseRepository, IPartnerRepository
    {
        public PartnerDatabaseRepository(POSContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        private readonly IMapper _mapper;

        public async Task Create(PartnerDTO data)
        {
            var model = _mapper.Map<Partner>(data);
            await dbContext.Partners.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(PartnerDTO data)
        {
            var model = await dbContext.Partners.FindAsync(data.Id);
            _mapper.Map(data, model);
            await dbContext.SaveChangesAsync();
        }

        public async Task<PartnerDTO> GetById(int id)
        {
            var model = await dbContext.Partners.FindAsync(id);
            if (model == null) return null;
            return _mapper.Map<PartnerDTO>(model);
        }

        public async Task<IEnumerable<PartnerDTO>> GetList()
        {
            var query = dbContext.Partners.Where(m => !m.IsDeleted);
            return await query.ProjectTo<PartnerDTO>(_mapper.ConfigurationProvider)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<PartnerResult> GetList(int? partnerTypeId = null, int page = 1)
        {
            var query = dbContext.Partners.Where(m => !m.IsDeleted);
            if (partnerTypeId != null) query = query.Where(m => m.PartnerTypeId == partnerTypeId);

            var rowCount = await query.CountAsync();
            var pageCount = Math.Ceiling((double)rowCount / pageSize);

            var data = await query.ProjectTo<PartnerDTO>(_mapper.ConfigurationProvider)
                .OrderByDescending(p => p.UpdatedDate)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .OrderByDescending(x => x.UpdatedDate)
                .ToListAsync();

            return new PartnerResult { Partners = data, PageCount = (int)pageCount };
        }

        public async Task Delete(int[] ids)
        {
            await dbContext.Partners
                .Where(m => ids.Any(id => m.Id == id))
                .ExecuteUpdateAsync(m => m.SetProperty(p => p.IsDeleted, p => true));
        }
    }
}
