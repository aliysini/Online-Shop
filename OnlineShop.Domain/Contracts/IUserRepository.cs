using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Contracts
{
    public interface IUserRepository : OnlineShop.Common.Contract.IReadRepository<User>,OnlineShop.Common.Contract.IWriteRepository<User>
    {
        public Task<User> GetByUserNameAsync(string name);
        public Task<IEnumerable<User>> GetAllAsync();
        public Task<User> GetByIdAsync(int id);
        public Task DeleteAsync(User entity);
        public Task UpdateAsync(User entity);
        public Task AddAsync(User entity);
        public Task SaveChangeAsync();



    }
}
