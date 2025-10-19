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
    public class InvoiceDatabaseRepository : BaseDatabaseRepository, IInvoiceRepository
    {
        public InvoiceDatabaseRepository(POSContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        private readonly IMapper _mapper;

        public async Task Create(InvoiceDTO data)
        {
            var model = _mapper.Map<Invoice>(data);
            await dbContext.Invoices.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(InvoiceDTO data)
        {
            var model = await dbContext.Invoices.FindAsync(data.Id);
            _mapper.Map(data, model);
            await dbContext.SaveChangesAsync();
        }

        public async Task<InvoiceDTO> GetById(int id)
        {
            var row = await dbContext.Invoices
                .Include(i => i.InvoiceItems).ThenInclude(i => i.Product)
                .Include(i => i.Currency)
                .SingleOrDefaultAsync(i => i.Id == id);

            return _mapper.Map<InvoiceDTO>(row);
        }

        public async Task<InvoiceResult> GetList(byte? invoiceType, DateTime? issueDate, int page = 1)
        {
            var query = dbContext.Invoices.Where(i => i.IsDeleted == false);
            if (invoiceType != null) query = query.Where(i => i.InvoiceType == invoiceType);
            if (issueDate != null) query = query.Where(i => i.IssueDate == issueDate);

            var rowCount = await query.CountAsync();
            var pageCount = Math.Ceiling((double)rowCount / pageSize);

            var data = await query.ProjectTo<InvoiceRowDTO>(_mapper.ConfigurationProvider)
                .OrderByDescending(p => p.UpdatedDate)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            return new InvoiceResult
            {
                Invoices = data,
                PageCount = (int)pageCount
            };
        }
    }
}
