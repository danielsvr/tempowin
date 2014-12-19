using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwinAndKatanaTry
{
    public interface IView
    {
        object Model { get; set; }
        string ViewName { get; set; }
    }
}