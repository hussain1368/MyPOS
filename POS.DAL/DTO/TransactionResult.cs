using System.Collections.Generic;

namespace POS.DAL.DTO
{
    public class TransactionResult
    {
        public IEnumerable<TransactionDTO> Transactions { get; set; }
        public int PageCount { get; set; }
    }
}
