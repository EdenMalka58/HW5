using System;
using System.Security.Cryptography.Xml;

namespace HW5.Models
{
    public class LoginUserResponse
    {
        User user;
        int error;

        public LoginUserResponse(User user, int error)
        {
            User = user;
            Error = error;
        }

        public LoginUserResponse() { }

        public User User { get => user; set => user = value; }
        public int Error { get => error; set => error = value; }
    }
}
