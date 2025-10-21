using POS.DAL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface IUserRepository
    {
        Task Create(UserDTO data);
        Task<UserDTO> Login(string username, string password);
        Task UpdatePassword(int id, string currentPassword, string newPassword);
        Task Update(UserDTO data);
        Task Delete(int[] ids);
        Task Restore(int[] ids);
        Task<UserResult> GetList(string role);
        Task<UserDTO> GetById(int id);
    }
}