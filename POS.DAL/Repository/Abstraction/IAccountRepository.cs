using POS.DAL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface IAccountRepository
    {
        Task Create(AccountDTO data);
        Task Delete(int[] ids);
        Task<AccountDTO> GetById(int id);
        Task<IEnumerable<AccountDTO>> GetList(int? accountTypeId = null);
        Task Update(AccountDTO data);
    }
}