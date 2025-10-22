using POS.DAL.DTO;
using System;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface ITransactionRepository
    {
        Task Create(TransactionDTO data);
        Task Delete(int[] ids);
        Task<TransactionDTO> GetById(int id);
        Task<TransactionResult> GetList(byte? transactionType = null, 
            int? partnerId = null, 
            int? sourceId = null,
            DateTime? date = null,
            int page = 1);
        Task Update(TransactionDTO data);
    }
}