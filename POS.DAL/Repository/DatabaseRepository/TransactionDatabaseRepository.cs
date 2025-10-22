using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.DTO;
using POS.DAL.Repository.Abstraction;
using System;
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

        public async Task Create(TransactionDTO data)
        {
            var model = mapper.Map<Transaction>(data);
            await dbContext.Transactions.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(TransactionDTO data)
        {
            var model = await dbContext.Transactions.FindAsync(data.Id);
            mapper.Map(data, model);
            await dbContext.SaveChangesAsync();
        }

        public async Task<TransactionDTO> GetById(int id)
        {
            var model = await dbContext.Transactions.FindAsync(id);
            if (model == null) return null;
            return mapper.Map<TransactionDTO>(model);
        }

        public async Task<TransactionResult> GetList(byte? transactionType = null, 
            int? partnerId = null, 
            int? sourceId = null,
            DateTime? date = null,
            int page = 1)
        {
            var query = dbContext.Transactions.AsNoTracking().Where(m => !m.IsDeleted);

            if (transactionType.HasValue)
                query = query.Where(m => m.TransactionType == transactionType);
            if (partnerId.HasValue)
                query = query.Where(m => m.PartnerId == partnerId);
            if (sourceId.HasValue)
                query = query.Where(m => m.SourceId == sourceId);
            if (date.HasValue)
                query = query.Where(m => m.Date.Date == date.Value.Date);

            var rowCount = await query.CountAsync();
            var pageCount = Math.Ceiling((double)rowCount / pageSize);

            var data = await query.ProjectTo<TransactionDTO>(mapper.ConfigurationProvider)
                .OrderByDescending(p => p.Date)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            return new TransactionResult { Transactions = data, PageCount = (int)pageCount };
        }

        public async Task Delete(int[] ids)
        {
            var rows = await dbContext.Transactions.Where(m => ids.Any(id => m.Id == id))
                .ExecuteUpdateAsync(m => m.SetProperty(p => p.IsDeleted, p => true));
        }
    }
}
