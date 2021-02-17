using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace POS.DAL.Query
{
    public class AccountQuery : DbQuery
    {
        public AccountQuery(POSContext dbContext) : base(dbContext) { }

        public async Task Create(AccountDTM data)
        {
            var model = new Account();
            MapSingle(data, model);
            await dbContext.Accounts.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(AccountDTM data)
        {
            var model = await dbContext.Accounts.FindAsync(data.Id);
            MapSingle(data, model);
            await dbContext.SaveChangesAsync();
        }

        private void MapSingle(AccountDTM data, Account model)
        {
            model.Name = data.Name;
            model.Phone = data.Phone;
            model.Address = data.Address;
            model.Note = data.Note;
            model.CurrencyId = data.CurrencyId;
            model.CurrentBalance = data.CurrentBalance;
            model.AccountType = data.AccountType;
            model.InsertedBy = data.InsertedBy;
            model.InsertedDate = data.InsertedDate;
            model.UpdatedBy = data.UpdatedBy;
            model.UpdatedDate = data.UpdatedDate;
            model.IsDeleted = data.IsDeleted;
        }

        public async Task<AccountDTM> GetById(int id)
        {
            var model = await dbContext.Accounts.FindAsync(id);
            if (model == null) return null;
            return new AccountDTM
            {
                Name = model.Name,
                Phone = model.Phone,
                Address = model.Address,
                Note = model.Note,
                CurrencyId = model.CurrencyId,
                CurrentBalance = model.CurrentBalance,
                AccountType = model.AccountType,
                InsertedBy = model.InsertedBy,
                InsertedDate = model.InsertedDate,
                UpdatedBy = model.UpdatedBy,
                UpdatedDate = model.UpdatedDate,
                IsDeleted = model.IsDeleted,
            };
        }

        public async Task<IEnumerable<AccountDTM>> GetList()
        {
            return await dbContext.Accounts.Where(x => !x.IsDeleted)
            .Select(x => new AccountDTM
            {
                Name = x.Name,
                Phone = x.Phone,
                Address = x.Address,
                Note = x.Note,
                CurrencyId = x.CurrencyId,
                CurrentBalance = x.CurrentBalance,
                AccountType = x.AccountType,
                InsertedBy = x.InsertedBy,
                InsertedDate = x.InsertedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                IsDeleted = x.IsDeleted,
                CurrencyName = x.Currency.Name,
                CurrencyCode = x.Currency.Code,
            })
            .ToListAsync();
        }
    }
}
