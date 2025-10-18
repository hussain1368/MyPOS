using POS.DAL.DTO;
using System;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface IInvoiceRepository
    {
        Task Create(InvoiceDTO data);
        Task<InvoiceDTO> GetById(int id);
        Task<InvoiceResult> GetList(byte? invoiceType, DateTime? issueDate, int page);
        Task Update(InvoiceDTO data);
    }
}