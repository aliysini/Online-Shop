using OnlineShop.Common.Contract;
using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Contracts
{
    public interface IProductRepository :IReadRepository<Product>,IWriteRepository<Product>
    {
        public Task<Product> GetByProductNameAsync(string name);
    }
}
