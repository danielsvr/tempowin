﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinAndKatanaTry
{
    public abstract class RequestHandlerBase
    {
        protected IDictionary<string, object> Environment { get; private set; }
        protected IEnumerable<MyAppRoute> Routes { get; private set; }

        protected string RequestPath
        {
            get { return (string)this.Environment["owin.RequestPath"]; }
            set { this.Environment["owin.RequestPath"] = value; }
        }

        protected string RequestQueryString
        {
            get { return (string)this.Environment["owin.RequestQueryString"]; }
            set { this.Environment["owin.RequestQueryString"] = value; }
        }

        public RequestHandlerBase(IDictionary<string, object> env, IEnumerable<MyAppRoute> routes)
        {
            this.Environment = env;
            this.Routes = routes;
            this.InitResponseType();
        }

        private void InitResponseType()
        {
            var header = (IDictionary<string, string[]>)this.Environment["owin.ResponseHeaders"];
            if (header.ContainsKey("Content-Type"))
                header["Content-Type"] = new[] { "text/html" };
            else
                header.Add("Content-Type", new[] { "text/html" });
        }

        public abstract Task<object> Handle();

        protected string GetViewPath(string controller, string viewName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "View", controller, viewName + ".cshtml");
        }

        protected async Task WriteResponse(string viewPath, object model)
        {
            if (!File.Exists(viewPath))
                throw new Exception("View not found. Path: " + viewPath);

            using (var writer = new StreamWriter((Stream)this.Environment["owin.ResponseBody"]))
            {
                await writer.WriteAsync(RazorEngine.Razor.Parse(new StreamReader(viewPath).ReadToEnd(), model));
            }
        }

        protected MyAppRoute GetRoute(string routeName)
        {
            var route = this.Routes.FirstOrDefault(x => x.Name.ToLower() == routeName.ToLower());
            if (route == null)
                throw new Exception("Route not found: " + routeName);

            return route;
        }

        protected Task<IView> InvokeController(Type controller, string actionName, string prefix = "Get")
        {
            var parts = actionName.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
            actionName = parts[0];
            var urlParams = !string.IsNullOrWhiteSpace(RequestQueryString) ?
                RequestQueryString.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                : new string[0];

            var controllerInstance = Activator.CreateInstance(controller, false);
            var actionMethod = GetMethod(controller, prefix + actionName);

            if (actionMethod == null)
                actionMethod = GetMethod(controller, actionName);

            if (actionMethod == null)
                throw new Exception("Action not found: " + actionName);

            var result = actionMethod.Invoke(controllerInstance,
                urlParams.Select(tuple => tuple.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1])
                         .Cast<object>().ToArray());
            if (result == null)
                result = Task.FromResult<IView>(null);

            if (result is IView)
                result = Task.FromResult<IView>((IView)result);

            return (Task<IView>)result;
        }

        private static System.Reflection.MethodInfo GetMethod(Type controller, string actionName)
        {
            var actionMethod = controller.GetMethod(actionName);
            if (actionMethod == null)
                foreach (var method in controller.GetMethods())
                {
                    if (method.Name.ToLower() == actionName.ToLower())
                        return method;
                }
            return actionMethod;
        }

        protected string[] GetControllerAndAction()
        {
            var result = new string[2];
            var requestPath = this.RequestPath.Substring(1).Split('/');

            result[0] = requestPath[0];
            result[1] = (requestPath.Length > 1) ? requestPath[1] : "Index";

            return result;
        }
    }
}