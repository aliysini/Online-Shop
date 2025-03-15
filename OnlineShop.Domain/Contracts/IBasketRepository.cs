using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasketAsync(string username);
        Task<ShoppingCart> CreateBasketAsync(ShoppingCart basket);
        Task DeleteBasketAsync(string username);
    }
}
