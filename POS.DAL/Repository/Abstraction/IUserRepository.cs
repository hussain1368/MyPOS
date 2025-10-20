using POS.DAL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface IUserRepository
    {
        Task Create(UserDTO data);
        Task<UserDTO> Login(string username, string password);
        Task UpdatePassword(int id, string password);
        Task Update(UserDTO data);
        Task DeleteUsers(int[] ids);
        Task RestoreUsers(int[] ids);
        Task<UserResult> GetList();
        Task<UserDTO> GetById(int id);
    }
}