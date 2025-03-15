using Microsoft.Extensions.Caching.Distributed;
using OnlineShop.Domain.Contracts;
using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnlineShop.Persistance.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        public BasketRepository(IDistributedCache distributedCache)
        {
            _redisCache = distributedCache;
        }
        private readonly IDistributedCache _redisCache;
        public async Task DeleteBasketAsync(string username)
        {
            await _redisCache.RemoveAsync(username);
        }
        public async Task<ShoppingCart> GetBasketAsync(string username)
        {
            var basket =await _redisCache.GetStringAsync(username);
            if (string.IsNullOrEmpty(basket)) 
            {
                return null;
            }

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> CreateBasketAsync(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.Username,JsonSerializer.Serialize(basket));
            return await GetBasketAsync(basket.Username);
        }
    }
}
