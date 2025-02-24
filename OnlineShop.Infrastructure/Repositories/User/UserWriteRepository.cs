using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OnlineShop.Application.Interfaces;
using OnlineShop.Persistance.Context.OnlineShopDbContext;

namespace OnlineShop.Infrastructure.Repositories.User
{
    public class UserWriteRepository : IWriteRepository<Domain.Entity.User>
    {
        public UserWriteRepository(OnlineShopDbContext onlineShopDb)
        {
                _OnlineShopDbContext=onlineShopDb;
        }
        private readonly OnlineShopDbContext _OnlineShopDbContext;

        public async Task AddAsync(Domain.Entity.User entity)
        {
             await _OnlineShopDbContext.Users.AddAsync(entity);
            await _OnlineShopDbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Domain.Entity.User entity)
        {
            _OnlineShopDbContext.Users.Update(entity); 
            await _OnlineShopDbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Domain.Entity.User entity)
        {
             _OnlineShopDbContext.Users.Remove(entity);
            await _OnlineShopDbContext.SaveChangesAsync();
        }
    }
}
