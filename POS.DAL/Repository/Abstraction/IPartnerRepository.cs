using POS.DAL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface IPartnerRepository
    {
        Task Create(PartnerDTO data);
        Task Delete(int[] ids);
        Task<PartnerDTO> GetById(int id);
        Task<IEnumerable<PartnerDTO>> GetList();
        Task<PartnerResult> GetList(int? partnerTypeId = null, int page = 1);
        Task Update(PartnerDTO data);
    }
}