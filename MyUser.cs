using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OwinAndKatanaTry
{
    public class MyUser : IUser<string>
    {
        public string Id
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }
    }
}
