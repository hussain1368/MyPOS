using POS.DAL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface IProductRepository
    {
        Task<bool> CheckDuplicate(int id, string code);
        Task Create(ProductDTO data);
        Task Delete(int[] ids);
        Task<ProductItemDTO> GetByCode(string code);
        Task<ProductDTO> GetById(int id);
        Task<IEnumerable<ProductItemDTO>> GetByName(string searchValue);
        Task<ProductResult> GetList(int? categoryId, int page = 1);
        Task Update(ProductDTO data);
    }
}