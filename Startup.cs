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
                        dataProtectionProvider.Create("ASP.NET Identity"));
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

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            //app.Use(new Func<object, object>(
            //    x => new Middleware(new List<MyAppRoute> 
            //    {
            //        // Your routes
            //        new MyAppRoute("Person", typeof(PersonController)),
            //        new MyAppRoute("Account", typeof(AccountController))
            //    })));

            app.CreatePerOwinContext<Middleware>(() => new Middleware(new List<MyAppRoute> 
            {
                // Your routes
                new MyAppRoute("Person", typeof(PersonController)),
                new MyAppRoute("Account", typeof(AccountController))
            }));

            //app.Use((ctx, next) =>
            //{
            //    //next();
            //    var middleware = ctx.Get<Middleware>();
            //    middleware.Invoke(ctx.Environment);
            //    return next.Invoke();
            //});

            app.Run(ctx =>
            {
                var middleware = ctx.Get<Middleware>();
                return middleware.Invoke(ctx.Environment);
            });

            //app.Run(Run);
        }

        //protected virtual Task Run(IOwinContext context)
        //{
        //    // Throw an exception for this URI path.
        //    if (context.Request.Path.Equals(new PathString("/fail")))
        //    {
        //        throw new Exception("Random exception");
        //    }

        //    if (context.Request.Path.Equals(new PathString("/")))
        //    {
        //        context.Response.ContentType = "text/plain";
        //        return context.Response.WriteAsync("Hello, world.");
        //    }

        //    context.Response.StatusCode = 500;
        //    //context.Response.Body.Close();
        //    //context.Response.Body = Stream.Null;
        //    return Task.FromResult((object)null);
        //}
    }

    public class DebugStartup : ProdStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            // Add the error page middleware to the pipeline. 
            app.UseErrorPage();

            base.Configuration(app);
        }

        //protected override Task Run(IOwinContext context)
        //{
        //    return base.Run(context);
        //}
    }
}
