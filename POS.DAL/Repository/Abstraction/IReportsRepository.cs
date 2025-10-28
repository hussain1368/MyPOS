using POS.DAL.DTO;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface IReportsRepository
    {
        Task<TotalsDTO> GetTotals();
    }
}