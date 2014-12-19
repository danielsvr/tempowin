using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwinAndKatanaTry
{
    public class MyAppRoute
    {
        public Type Controller { get; private set; }
        public string Name { get; private set; }

        public MyAppRoute(string name, Type controller)
        {
            this.Name = name;
            this.Controller = controller;
        }
    }
}