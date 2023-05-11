using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.DTO;
using POS.DAL.Repository.Abstraction;
using System;
using System.Threading.Tasks;

namespace POS.DAL.Repository.DatabaseRepository
{
    public class UserDatabaseRepository : BaseDatabaseRepository, IUserRepository
    {
        public UserDatabaseRepository(POSContext dbContext) : base(dbContext) { }

        public async Task Create(UserDTO data)
        {
            var user = new AppUser
            {
                Username = data.Username,
                Password = data.Password,
                DisplayName = data.DisplayName,
                UserRole = data.UserRole,
                UpdatedBy = data.UpdatedBy,
                UpdatedDate = data.UpdatedDate,
                IsDeleted = false,
            };
            var hasher = new PasswordHasher<AppUser>();
            user.Password = hasher.HashPassword(user, data.Password);
            await dbContext.AppUsers.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdatePassword(int id, string password)
        {
            var user = await dbContext.AppUsers.FindAsync(id);
            if (user == null) return;
            var hasher = new PasswordHasher<AppUser>();
            user.Password = hasher.HashPassword(user, password);
            await dbContext.SaveChangesAsync();
        }

        public async Task<UserDTO> Login(string username, string password)
        {
            var user = await dbContext.AppUsers.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null) throw new ApplicationException("Incorrect Username!");
            var hasher = new PasswordHasher<AppUser>();
            var result = hasher.VerifyHashedPassword(user, user.Password, password);
            if (result != PasswordVerificationResult.Success) throw new ApplicationException("Incorrect Password!");
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                DisplayName = user.DisplayName,
                UserRole = user.UserRole,
                UpdatedBy = user.UpdatedBy,
                UpdatedDate = user.UpdatedDate,
            };
        }
    }
}
