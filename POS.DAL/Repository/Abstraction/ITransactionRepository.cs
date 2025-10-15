using POS.DAL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface ITransactionRepository
    {
        Task Create(AccountDTO data);
        Task Delete(int[] ids);
        Task<AccountDTO> GetById(int id);
        Task<IEnumerable<TransactionDTO>> GetList();
        Task Update(AccountDTO data);
    }
}