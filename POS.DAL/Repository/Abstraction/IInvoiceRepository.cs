using POS.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface IInvoiceRepository
    {
        Task Create(InvoiceDTO data);
        Task<InvoiceDTO> GetById(int id);
        Task<IEnumerable<InvoiceRowDTO>> GetList(byte? invoiceType, DateTime? issueDate);
        Task Update(InvoiceDTO data);
    }
}