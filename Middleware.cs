﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinAndKatanaTry
{
    public class Middleware : IDisposable
    {
        private IEnumerable<MyAppRoute> Routes { get; set; }

        public Middleware(IEnumerable<MyAppRoute> routes)
        {
            this.Routes = routes;
        }

        public async Task<object> Invoke(IDictionary<string, object> env)
        {
            var handler = RequestFactory.Get(env, this.Routes);
            await handler.Handle();

            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
        }
    }
}