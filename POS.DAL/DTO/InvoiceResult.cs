using System.Collections.Generic;

namespace POS.DAL.DTO
{
    public class InvoiceResult
    {
        public IEnumerable<InvoiceRowDTO> Invoices { get; set; }
        public int PageCount { get; set; }
    }
}
