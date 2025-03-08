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
    public class ProductRepository : IProductRepository
    {
        public ProductRepository(OnlineShopDbContext onlineShopDbContext)
        {
            _onlineShopDbContext = onlineShopDbContext;
        }
        private readonly OnlineShopDbContext _onlineShopDbContext;
        public async Task AddAsync(Product entity)
        {
            await _onlineShopDbContext.Products.AddAsync(entity);
            await SaveChangeAsync();
        }

        public async Task DeleteAsync(Product entity)
        {
            entity.IsDeleted = true;
            await SaveChangeAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _onlineShopDbContext.Products
                .Where(current=> current.IsDeleted == false)
                .Include(current=> current.Category)
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _onlineShopDbContext.Products
                .Where (current=> current.Id == id)
                .Where(current=> current.IsDeleted == false)
                .Include(current=> current.Category)
                .FirstOrDefaultAsync();
        }

        public async Task<Product> GetByProductNameAsync(string name)
        {
            return await _onlineShopDbContext.Products
                .Where(current=> current.Name.ToLower() == name.ToLower())
                .Where(current=> current.IsDeleted == false)
                .Include(current=> current.Category)
                .FirstOrDefaultAsync();
        }
        public async Task SaveChangeAsync()
        {
            await _onlineShopDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _onlineShopDbContext.Products.Update(entity);
            await SaveChangeAsync();
        }
        public async Task UpdateProductsAsync(IEnumerable<Product> products)
        {
            _onlineShopDbContext.Products.UpdateRange(products);
            await SaveChangeAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int id)
        {
            var products = await _onlineShopDbContext.Products.Where(current => current.CategoryId == id).
                ToListAsync();
            return products;
        }
    }
}
