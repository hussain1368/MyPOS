using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POS.DAL.Domain;
using POS.DAL.DTO;
using POS.DAL.Repository.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.DAL.Repository.DatabaseRepository
{
    public class UserDatabaseRepository : BaseDatabaseRepository, IUserRepository
    {
        public UserDatabaseRepository(POSContext dbContext) : base(dbContext) { }

        private readonly PasswordHasher<AppUser> hasher = new PasswordHasher<AppUser>();

        public async Task Create(UserDTO data)
        {
            var user = new AppUser
            {
                Username = data.Username.ToLower(),
                Password = data.Password,
                DisplayName = data.DisplayName,
                UserRole = data.UserRole,
                UpdatedBy = data.UpdatedBy,
                UpdatedDate = data.UpdatedDate,
                IsDeleted = false,
            };
            user.Password = hasher.HashPassword(user, data.Password);
            await dbContext.AppUsers.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(UserDTO data)
        {
            var model = await dbContext.AppUsers.FirstOrDefaultAsync(m => m.Id == data.Id);
            model.DisplayName = data.DisplayName;
            model.UserRole = data.UserRole;
            model.UpdatedBy = data.UpdatedBy;
            model.UpdatedDate = data.UpdatedDate;
            if (!string.IsNullOrEmpty(data.Password) /* later add user role check here */)
            {
                model.Password = hasher.HashPassword(model, data.Password);
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdatePassword(int id, string password)
        {
            var user = await dbContext.AppUsers.FindAsync(id);
            if (user == null) return;
            user.Password = hasher.HashPassword(user, password);
            await dbContext.SaveChangesAsync();
        }

        public async Task<UserDTO> GetById(int id)
        {
            var user = await dbContext.AppUsers.AsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    DisplayName = u.DisplayName,
                    UserRole = u.UserRole,
                    UpdatedBy = u.UpdatedBy,
                    UpdatedDate = u.UpdatedDate,
                })
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<UserResult> GetList(string role)
        {
            var query = dbContext.AppUsers.AsNoTracking();

            if (!string.IsNullOrEmpty(role)) query = query.Where(u => u.UserRole == role);

            var users = await query.Select(u => new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    DisplayName = u.DisplayName,
                    UserRole = u.UserRole,
                    UpdatedBy = u.UpdatedBy,
                    UpdatedDate = u.UpdatedDate,
                    IsDeleted = u.IsDeleted,
                })
                .ToListAsync();
            return new UserResult { Users = users };
        }

        public async Task<UserDTO> Login(string username, string password)
        {
            var user = await dbContext.AppUsers.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null) throw new ApplicationException("The username is wrong!");
            var result = hasher.VerifyHashedPassword(user, user.Password, password);
            if (result != PasswordVerificationResult.Success) throw new ApplicationException("The password is wrong!");
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

        public async Task Delete(int[] ids)
        {
            var rows = await dbContext.AppUsers.Where(m => ids.Any(id => m.Id == id))
                .ExecuteUpdateAsync(m => m.SetProperty(m => m.IsDeleted, true));
        }

        public async Task Restore(int[] ids)
        {
            var rows = await dbContext.AppUsers.Where(m => ids.Any(id => m.Id == id))
                .ExecuteUpdateAsync(m => m.SetProperty(m => m.IsDeleted, false));
        }
    }
}
