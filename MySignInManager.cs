using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OwinAndKatanaTry
{
    class MySignInManager : SignInManager<MyUser, string>
    {
        public MySignInManager(MyUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { 
        }

        public override async Task<ClaimsIdentity> CreateUserIdentityAsync(MyUser user)
        {
            var userIdentity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

    }
}
