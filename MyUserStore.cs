using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinAndKatanaTry
{
    public class MyUserStore : IUserStore<MyUser>
    {
        public Task CreateAsync(MyUser user)
        {
            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(MyUser user)
        {
            return Task.FromResult<object>(null);
        }

        public Task<MyUser> FindByIdAsync(string userId)
        {
            return Task.FromResult(new MyUser { Id = userId, UserName = userId });
        }

        public Task<MyUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(new MyUser { Id = userName, UserName = userName });
        }

        public Task UpdateAsync(MyUser user)
        {
            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
        }
    }
}
