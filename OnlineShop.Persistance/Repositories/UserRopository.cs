using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entity;
using OnlineShop.Persistance.Context.OnlineShopDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Persistance.Repositories
{
    public class UserRopository : OnlineShop.Domain.Contracts.IUserRepository
    {
        public UserRopository(OnlineShopDbContext onlineShopDbContext)
        {
            _OnlineShopDbContext = onlineShopDbContext;
        }
        private readonly OnlineShopDbContext _OnlineShopDbContext;

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _OnlineShopDbContext.Users
                .Where(current => current.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {

            return await _OnlineShopDbContext.Users
                .Where(current => current.Id == id)
                .Where(current => current.IsDeleted == false)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByUserNameAsync(string name)
        {
            return await _OnlineShopDbContext.Users
                .Where(current => current.UserName == name).FirstOrDefaultAsync();
        }

        public async Task AddAsync(User entity)
        {
           await _OnlineShopDbContext.Users.AddAsync(entity);
            await SaveChangeAsync();
        }

        public async Task UpdateAsync(User entity)
        {
             _OnlineShopDbContext.Users.Update(entity);
             await SaveChangeAsync();
        }

        public async Task DeleteAsync(User entity)
        {
            entity.IsDeleted = true;
            await SaveChangeAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _OnlineShopDbContext.SaveChangesAsync();
        }
    }
}
