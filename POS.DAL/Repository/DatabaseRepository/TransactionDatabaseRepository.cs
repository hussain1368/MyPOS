using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.DTO;
using POS.DAL.Repository.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.DAL.Repository.DatabaseRepository
{
    public class TransactionDatabaseRepository : BaseDatabaseRepository, ITransactionRepository
    {
        private readonly IMapper mapper;

        public TransactionDatabaseRepository(POSContext dbContext, IMapper mapper) : base(dbContext)
        {
            this.mapper = mapper;
        }

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

        public async Task<IEnumerable<TransactionDTO>> GetList()
        {
            var query = dbContext.Transactions.Where(m => !m.IsDeleted);
            return await query.ProjectTo<TransactionDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task Delete(int[] ids)
        {
            var rows = await dbContext.Accounts.Where(m => ids.Any(id => m.Id == id)).ToListAsync();
            foreach (var row in rows) row.IsDeleted = true;
            await dbContext.SaveChangesAsync();
        }
    }
}
