using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwinAndKatanaTry
{
    public class RequestFactory
    {
        public static RequestHandlerBase Get(IDictionary<string, object> env, IEnumerable<MyAppRoute> routes)
        {
            var httpMethod = ((string)env["owin.RequestMethod"]).ToUpper();

            switch (httpMethod)
            {
                case "GET": return new GetHandler(env, routes);
                case "POST": return new PostHandler(env, routes);
                case "PUT": throw new NotImplementedException("POST handler");
                default: throw new NotImplementedException("No handler found for: " + httpMethod);
            }
        }
    }
}