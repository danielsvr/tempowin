using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinAndKatanaTry
{
    public class MyUserStore : 
        IUserStore<MyUser>,
        IUserLockoutStore<MyUser, string>,
        IUserPasswordStore<MyUser>,
        IUserTwoFactorStore<MyUser, string>
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

        public Task<int> GetAccessFailedCountAsync(MyUser user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(MyUser user)
        {
            return Task.FromResult(false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(MyUser user)
        {
            return Task.FromResult(DateTimeOffset.MinValue);
        }

        public Task<int> IncrementAccessFailedCountAsync(MyUser user)
        {
            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(MyUser user)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetLockoutEnabledAsync(MyUser user, bool enabled)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetLockoutEndDateAsync(MyUser user, DateTimeOffset lockoutEnd)
        {
            return Task.FromResult<object>(null);
        }


        public Task<string> GetPasswordHashAsync(MyUser user)
        {
            return Task.FromResult(Convert.ToBase64String(new byte[] { 1, 2, 3 }));
        }

        public Task<bool> HasPasswordAsync(MyUser user)
        {
            return Task.FromResult(false);
        }

        public Task SetPasswordHashAsync(MyUser user, string passwordHash)
        {
            return Task.FromResult<object>(null);
        }

        public Task<bool> GetTwoFactorEnabledAsync(MyUser user)
        {
            return Task.FromResult(false);
        }

        public Task SetTwoFactorEnabledAsync(MyUser user, bool enabled)
        {
            return Task.FromResult<object>(null);
        }
    }
}
