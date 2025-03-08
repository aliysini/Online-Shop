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
        public async Task DeleteAsync(Category entity)
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

        public async Task<Category> GetByCategoryNameAsync(string name)
        {
            return await _onlineShopDbContext.Categories
                .Where(current => current.Name.ToLower() == name.ToLower())
                .Where(current => current.IsDeleted == false)
                .FirstOrDefaultAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _onlineShopDbContext.Categories
                .Where(current => current.Id == id)
                .Where(current => current.IsDeleted == false)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Category entity)
        {
            _onlineShopDbContext.Categories.Update(entity);
            await SaveChangeAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _onlineShopDbContext.SaveChangesAsync();
        }

        public async Task<Category> GetProdectsAsync(Category entity)
        {
            var produucts = await _onlineShopDbContext.Categories.Where(current => current.Id == entity.Id)
                .Include(current => current.Products).FirstOrDefaultAsync();
            return produucts;
        }
        public async Task IsDeleteAsync(Category entity)
        {
             _onlineShopDbContext.Categories.Remove(entity);
            await SaveChangeAsync();
        }
    }
}
