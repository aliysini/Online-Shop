using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.Entity;
using OnlineShop.Persistance.Context.OnlineShopDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Infrastructure.Repositories.User
{
    public class UserReadRepository : IReadRepository<Domain.Entity.User>
    {
        public UserReadRepository(OnlineShopDbContext onlineShopDb)
        {
            _OnlineShopDbContext = onlineShopDb;
        }
        private readonly OnlineShopDbContext _OnlineShopDbContext;
        public async Task<IEnumerable<Domain.Entity.User>> GetAllAsync()
        {
            return  await _OnlineShopDbContext.Users
                .Where(current=>current.IsDeleted==false)
                .ToListAsync();
        }

        public async Task<Domain.Entity.User> GetByIdAsync(int id)
        {
            return await _OnlineShopDbContext.Users
                .Where(current => current.Id == id)
                .FirstOrDefaultAsync();
        }

    }
}
