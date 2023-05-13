using POS.DAL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface ICurrencyRateRepository
    {
        Task Create(CurrencyRateDTO data);
        Task Delete(int[] ids);
        Task<IEnumerable<CurrencyRateDTO>> GetList(int? currencyId);
        Task Update(CurrencyRateDTO data);
    }
}