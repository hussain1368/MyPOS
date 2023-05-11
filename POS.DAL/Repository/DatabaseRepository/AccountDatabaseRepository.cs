using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using POS.DAL.Repository.Abstraction;

namespace POS.DAL.Repository.DatabaseRepository
{
    public class AccountDatabaseRepository : BaseDatabaseRepository, IAccountRepository
    {
        public AccountDatabaseRepository(POSContext dbContext) : base(dbContext) { }

        public async Task Create(AccountDTO data)
        {
            var model = new Account();
            MapSingle(data, model);
            await dbContext.Accounts.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(AccountDTO data)
        {
            var model = await dbContext.Accounts.FindAsync(data.Id);
            MapSingle(data, model);
            await dbContext.SaveChangesAsync();
        }

        private void MapSingle(AccountDTO data, Account model)
        {
            model.Id = data.Id;
            model.Name = data.Name;
            model.Phone = data.Phone;
            model.Address = data.Address;
            model.Note = data.Note;
            model.CurrencyId = data.CurrencyId;
            model.CurrentBalance = data.CurrentBalance;
            model.AccountTypeId = data.AccountTypeId;
            model.UpdatedBy = data.UpdatedBy;
            model.UpdatedDate = data.UpdatedDate;
            model.IsDeleted = data.IsDeleted;
        }

        public async Task<AccountDTO> GetById(int id)
        {
            var model = await dbContext.Accounts.FindAsync(id);
            if (model == null) return null;
            return new AccountDTO
            {
                Id = model.Id,
                Name = model.Name,
                Phone = model.Phone,
                Address = model.Address,
                Note = model.Note,
                CurrencyId = model.CurrencyId,
                CurrentBalance = model.CurrentBalance,
                AccountTypeId = model.AccountTypeId,
                UpdatedBy = model.UpdatedBy,
                UpdatedDate = model.UpdatedDate,
                IsDeleted = model.IsDeleted,
            };
        }

        public async Task<IEnumerable<AccountDTO>> GetList(int? accountTypeId = null)
        {
            var query = dbContext.Accounts.Where(m => !m.IsDeleted);
            if (accountTypeId != null) query = query.Where(m => m.AccountTypeId == accountTypeId);
            return await query.Select(x => new AccountDTO
            {
                Id = x.Id,
                Name = x.Name,
                Phone = x.Phone,
                Address = x.Address,
                Note = x.Note,
                CurrencyId = x.CurrencyId,
                CurrentBalance = x.CurrentBalance,
                AccountTypeId = x.AccountTypeId,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                IsDeleted = x.IsDeleted,
                AccountTypeName = x.AccountType.Name,
                CurrencyName = x.Currency.Name,
                CurrencyCode = x.Currency.Code,
            })
            .ToListAsync();
        }

        public async Task Delete(int[] ids)
        {
            var rows = await dbContext.Accounts.Where(m => ids.Any(id => m.Id == id)).ToListAsync();
            foreach (var row in rows) row.IsDeleted = true;
            await dbContext.SaveChangesAsync();
        }
    }
}
