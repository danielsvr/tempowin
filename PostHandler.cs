using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinAndKatanaTry
{
    public class PostHandler: RequestHandlerBase
    {
        public PostHandler(IDictionary<string, object> env, IEnumerable<MyAppRoute> routes) 
            : base(env, routes) 
        { }

        public async override Task<object> Handle()
        {
            var controllerAndAction =  base.GetControllerAndAction();
            var route = base.GetRoute(controllerAndAction[0]);
            var view = await base.InvokeController(route.Controller, controllerAndAction[1], "Post");
            if(view == null)
                return Task.FromResult<object>(null);

            var viewPath = base.GetViewPath(controllerAndAction[0], view.ViewName);
            await base.WriteResponse(viewPath, view.Model);

            return Task.FromResult<object>(null);
        }
    }
}
