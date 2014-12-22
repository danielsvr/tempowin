using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OwinAndKatanaTry
{
    class AccountController
    {
        public IView Login(string returnUrl)
        {
            return new View("Login", new LoginViewModel { ReturnUrl = returnUrl });
        }

        public async Task<IView> PostLogin(string returnUrl)
        {
            var context = HttpContext.Current.GetOwinContext();
            var manager = context.Get<MySignInManager>();
            var result = await manager.PasswordSignInAsync("my user", "my secret", 
                         /*remember me*/ isPersistent: false, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    {
                        context.Response.Redirect("/" + returnUrl);
                        return null; 
                    }
                case SignInStatus.LockedOut:
                    return null;
                case SignInStatus.RequiresVerification:
                    return null;
            }
            //SignInStatus.Failure:
            //default:
            //"Invalid login attempt"
            return null;
        }
    }
}
