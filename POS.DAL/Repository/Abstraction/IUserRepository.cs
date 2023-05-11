using POS.DAL.DTO;
using System.Threading.Tasks;

namespace POS.DAL.Repository.Abstraction
{
    public interface IUserRepository
    {
        Task Create(UserDTO data);
        Task<UserDTO> Login(string username, string password);
        Task UpdatePassword(int id, string password);
    }
}