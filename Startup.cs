using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup("ProdConf", typeof(OwinAndKatanaTry.ProdStartup))]
[assembly: OwinStartup("DebugConf", typeof(OwinAndKatanaTry.DebugStartup))]

namespace OwinAndKatanaTry
{
    public class ProdStartup
    {
        public virtual void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<MyUserStore>(() => new MyUserStore());
            app.CreatePerOwinContext<MyUserManager>((opt, ctx) =>
            {
                var manager = new MyUserManager(ctx.Get<MyUserStore>());
                var dataProtectionProvider = opt.DataProtectionProvider;
                if (dataProtectionProvider != null)
                {
                    manager.UserTokenProvider = new DataProtectorTokenProvider<MyUser>(
                        dataProtectionProvider.Create());
                    //dataProtectionProvider.Create("ASP.NET Identity"/*optional; by default it includes application name and Microsoft.Owin.Security.IDataProtector as purposes; everything is additional*/));
                }
                return manager;
            });
            app.CreatePerOwinContext<MySignInManager>((opt, ctx) =>
                new MySignInManager(ctx.GetUserManager<MyUserManager>(), ctx.Authentication)
            );

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<MyUserManager, MyUser>(
                        validateInterval: TimeSpan.FromMinutes(20),
                        regenerateIdentity: (manager, user) => manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie))
                }
            });

            app.CreatePerOwinContext<Middleware>(() => new Middleware(new List<MyAppRoute> 
            {
                // Your routes
                new MyAppRoute("Person", typeof(PersonController)),
                new MyAppRoute("Account", typeof(AccountController))
            }));

            app.Run(ctx =>
            {
                var middleware = ctx.Get<Middleware>();
                return middleware.Invoke(ctx.Environment);
            });
        }
    }

    public class DebugStartup : ProdStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            // Add the error page middleware to the pipeline. 
            app.UseErrorPage();

            base.Configuration(app);
        }
    }
}
