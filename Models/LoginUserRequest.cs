using System;
using System.Security.Cryptography.Xml;

namespace HW5.Models
{
    public class LoginUserRequest
    {
        string email;
        string password;
        public LoginUserRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
        public LoginUserRequest() { }

        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
    }
}
