using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OwinAndKatanaTry
{
    public class PersonController
    {
        public IView Index()
        {
            if (!HttpContext.Current.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
            {
                HttpContext.Current.GetOwinContext().Response.StatusCode = 401;
                HttpContext.Current.GetOwinContext().Response.Body.Close();
                HttpContext.Current.GetOwinContext().Response.Body = Stream.Null;
                return null;
            }

            return new View("Index", new Person() { Firstname = "Max" });
        }

        public IView All()
        {

            var result = new List<Person>();

            for (var x = 0; x < 150; x++)
            {
                result.Add(new Person() { Firstname = "Max " + x });
            }

            return new View("All", result);
        }
    }
}