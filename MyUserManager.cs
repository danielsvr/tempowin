using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OwinAndKatanaTry
{
    public class MyUserManager : UserManager<MyUser>
    {
        public MyUserManager(MyUserStore store)
            : base(store)
        {
        }
    }
}
