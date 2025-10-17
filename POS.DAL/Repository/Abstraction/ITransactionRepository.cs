using POS.DAL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface ITransactionRepository
    {
        Task Create(TransactionDTO data);
        Task Delete(int[] ids);
        Task<TransactionDTO> GetById(int id);
        Task<IEnumerable<TransactionDTO>> GetList(byte? transactionType = null, int? accountId = null, int? sourceId = null);
        Task Update(TransactionDTO data);
    }
}