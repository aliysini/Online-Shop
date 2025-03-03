using OnlineShop.Common.Contract;
using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Contracts
{
    public interface ICategoryRepository :IWriteRepository<Category>,IReadRepository<Category>
    {
        public Task<Category> GetByCategoryNameAsync(string name);
    }
}
