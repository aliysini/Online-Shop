using Microsoft.EntityFrameworkCore;
using OnlineShop.Common.Contract;
using OnlineShop.Domain.Contracts;
using OnlineShop.Domain.Entity;
using OnlineShop.Persistance.Context.OnlineShopDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Persistance.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public CategoryRepository(OnlineShopDbContext onlineShopDbContext)
        {
            _onlineShopDbContext = onlineShopDbContext; 
        }
        private readonly OnlineShopDbContext _onlineShopDbContext;
        public async Task AddAsync(Category entity)
        {
           await _onlineShopDbContext.AddAsync(entity);
            await SaveChangeAsync();
        }

        async Task IWriteRepository<Category>.DeleteAsync(Category entity)
        {
            entity.IsDeleted = true;
            await SaveChangeAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _onlineShopDbContext.Categories
                .Where(current => current.IsDeleted == false)
                .ToListAsync();
        }

        async Task<Category> ICategoryRepository.GetByCategoryNameAsync(string name)
        {
            return await _onlineShopDbContext.Categories
                .Where(current => current.Name.ToLower() == name.ToLower())
                .Where(current=> current.IsDeleted == false)
                .FirstOrDefaultAsync();
        }

        async Task<Category> IReadRepository<Category>.GetByIdAsync(int id)
        {
            return await _onlineShopDbContext.Categories
                .Where(current => current.Id == id)
                .Where(current => current.IsDeleted == false)
                .FirstOrDefaultAsync();
        }

        async Task IWriteRepository<Category>.UpdateAsync(Category entity)
        {
            _onlineShopDbContext.Categories.Update(entity);
            await SaveChangeAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _onlineShopDbContext.SaveChangesAsync();
        }

    }
}
