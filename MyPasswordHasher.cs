using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OwinAndKatanaTry
{
    class MyPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            //return Convert.ToBase64String(Encoding.ASCII.GetBytes(password));
            return Convert.ToBase64String(new byte[] { 1, 2, 3 });
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return hashedPassword == HashPassword(providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
