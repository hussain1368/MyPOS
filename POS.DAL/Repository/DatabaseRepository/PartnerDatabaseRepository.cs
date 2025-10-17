using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using POS.DAL.Repository.Abstraction;

namespace POS.DAL.Repository.DatabaseRepository
{
    public class PartnerDatabaseRepository : BaseDatabaseRepository, IPartnerRepository
    {
        public PartnerDatabaseRepository(POSContext dbContext) : base(dbContext) { }

        public async Task Create(PartnerDTO data)
        {
            var model = new Partner();
            MapSingle(data, model);
            await dbContext.Partners.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(PartnerDTO data)
        {
            var model = await dbContext.Partners.FindAsync(data.Id);
            MapSingle(data, model);
            await dbContext.SaveChangesAsync();
        }

        private void MapSingle(PartnerDTO data, Partner model)
        {
            model.Id = data.Id;
            model.Name = data.Name;
            model.Phone = data.Phone;
            model.Address = data.Address;
            model.Note = data.Note;
            model.CurrencyId = data.CurrencyId;
            model.CurrentBalance = data.CurrentBalance;
            model.PartnerTypeId = data.PartnerTypeId;
            model.UpdatedBy = data.UpdatedBy;
            model.UpdatedDate = data.UpdatedDate;
            model.IsDeleted = data.IsDeleted;
        }

        public async Task<PartnerDTO> GetById(int id)
        {
            var model = await dbContext.Partners.FindAsync(id);
            if (model == null) return null;
            return new PartnerDTO
            {
                Id = model.Id,
                Name = model.Name,
                Phone = model.Phone,
                Address = model.Address,
                Note = model.Note,
                CurrencyId = model.CurrencyId,
                CurrentBalance = model.CurrentBalance,
                PartnerTypeId = model.PartnerTypeId,
                UpdatedBy = model.UpdatedBy,
                UpdatedDate = model.UpdatedDate,
                IsDeleted = model.IsDeleted,
            };
        }

        public async Task<IEnumerable<PartnerDTO>> GetList(int? partnerTypeId = null)
        {
            var query = dbContext.Partners.Where(m => !m.IsDeleted);
            if (partnerTypeId != null) query = query.Where(m => m.PartnerTypeId == partnerTypeId);
            return await query.Select(x => new PartnerDTO
            {
                Id = x.Id,
                Name = x.Name,
                Phone = x.Phone,
                Address = x.Address,
                Note = x.Note,
                CurrencyId = x.CurrencyId,
                CurrentBalance = x.CurrentBalance,
                PartnerTypeId = x.PartnerTypeId,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                IsDeleted = x.IsDeleted,
                PartnerTypeName = x.PartnerType.Name,
                CurrencyName = x.Currency.Name,
                CurrencyCode = x.Currency.Code,
            })
            .ToListAsync();
        }

        public async Task Delete(int[] ids)
        {
            var rows = await dbContext.Partners.Where(m => ids.Any(id => m.Id == id)).ToListAsync();
            foreach (var row in rows) row.IsDeleted = true;
            await dbContext.SaveChangesAsync();
        }
    }
}
