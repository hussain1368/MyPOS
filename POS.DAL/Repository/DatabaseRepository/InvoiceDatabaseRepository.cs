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
        public InvoiceDatabaseRepository(POSContext dbContext) : base(dbContext) { }

        public async Task Create(InvoiceDTO data)
        {
            var model = new Invoice();
            MapSingle(data, model);
            await dbContext.Invoices.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(InvoiceDTO data)
        {
            var model = await dbContext.Invoices.FindAsync(data.Id);
            MapSingle(data, model);
            await dbContext.SaveChangesAsync();
        }

        private void MapSingle(InvoiceDTO data, Invoice model)
        {
            model.SerialNum = data.SerialNum;
            model.InvoiceType = data.InvoiceType;
            model.WarehouseId = data.WarehouseId;
            model.TreasuryId = data.TreasuryId;
            model.AccountId = data.AccountId;
            model.CurrencyId = data.CurrencyId;
            model.CurrencyRate = data.CurrencyRate;
            model.IssueDate = data.IssueDate;
            model.PaymentType = data.PaymentType;
            model.Note = data.Note;
            model.UpdatedBy = data.UpdatedBy;
            model.UpdatedDate = data.UpdatedDate;
            model.IsDeleted = false;
            model.ItemsCount = data.Items.Count();
            model.TotalPrice = data.Items.Sum(i => i.TotalPrice);

            model.InvoiceItems = data.Items.Select(i => new InvoiceItem
            {
                ProductId = i.ProductId,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice,
                Cost = i.Cost,
                Profit = i.Profit,
                UnitDiscount = i.UnitDiscount,
                TotalDiscount = i.TotalDiscount,
                Quantity = i.Quantity,
            })
            .ToList();
        }

        public async Task<InvoiceDTO> GetById(int id)
        {
            var row = await dbContext.Invoices
                .Include(i => i.InvoiceItems).ThenInclude(i => i.Product)
                .SingleOrDefaultAsync(i => i.Id == id);

            var invoice = new InvoiceDTO
            {
                Id = row.Id,
                SerialNum = row.SerialNum,
                InvoiceType = row.InvoiceType,
                WarehouseId = row.WarehouseId,
                TreasuryId = row.TreasuryId,
                AccountId = row.AccountId,
                CurrencyId = row.CurrencyId,
                CurrencyRate = row.CurrencyRate,
                IssueDate = row.IssueDate,
                PaymentType = row.PaymentType,
                Note = row.Note,
                UpdatedBy = row.UpdatedBy,
                UpdatedDate = row.UpdatedDate,
            };
            invoice.Items = row.InvoiceItems.Select(i => new InvoiceItemDTO
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                ProductCode = i.Product.Code,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice,
                Cost = i.Cost,
                Profit = i.Profit,
                UnitDiscount = i.UnitDiscount,
                TotalDiscount = i.TotalDiscount,
                Quantity = i.Quantity,
            })
            .ToList();
            return invoice;
        }

        public async Task<IEnumerable<InvoiceRowDTO>> GetList(byte? invoiceType, DateTime? issueDate)
        {
            var query = dbContext.Invoices.Where(i => i.IsDeleted == false);
            if (invoiceType != null) query = query.Where(i => i.InvoiceType == invoiceType);
            if (issueDate != null) query = query.Where(i => i.IssueDate == issueDate);
            return await query.Select(i => new InvoiceRowDTO
            {
                Id = i.Id,
                SerialNum = i.SerialNum,
                InvoiceType = i.InvoiceType,
                WarehouseId = i.WarehouseId,
                TreasuryId = i.TreasuryId,
                AccountId = i.AccountId,
                AccountName = i.Account != null ? i.Account.Name : "---",
                CurrencyId = i.CurrencyId,
                CurrencyRate = i.CurrencyRate,
                CurrencyCode = i.Currency.Code,
                CurrencyName = i.Currency.Name,
                IssueDate = i.IssueDate,
                PaymentType = i.PaymentType,
                UpdatedBy = i.UpdatedBy,
                UpdatedDate = i.UpdatedDate,
                ItemsCount = i.ItemsCount,
                TotalPrice = i.TotalPrice,
            })
            .ToListAsync();
        }
    }
}
