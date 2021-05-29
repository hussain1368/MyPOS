using POS.DAL.Domain;
using POS.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.DAL.Query
{
    public class InvoiceQuery : DbQuery
    {
        public InvoiceQuery(POSContext dbContext) : base(dbContext) { }

        public async Task Create(InvoiceDTO data)
        {
            var invoice = new Invoice
            {
                SerialNum = data.SerialNum,
                InvoiceType = data.InvoiceType,
                WarehouseId = data.WarehouseId,
                TreasuryId = data.TreasuryId,
                AccountId = data.AccountId,
                AccountName = data.AccountName,
                CurrencyId = data.CurrencyId,
                CurrencyRate = data.CurrencyRate,
                IssueDate = data.IssueDate,
                PaymentType = data.PaymentType,
                Note = data.Note,
                UpdatedBy = data.UpdatedBy,
                UpdatedDate = data.UpdatedDate,
                IsDeleted = false,
            };

            invoice.InvoiceItems = data.Items.Select(i => new InvoiceItem
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

            await dbContext.Invoices.AddAsync(invoice);
            await dbContext.SaveChangesAsync();
        }
    }
}
