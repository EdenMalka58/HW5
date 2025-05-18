using System;
using System.Security.Cryptography.Xml;

namespace HW5.Models
{
    public class ActiveUserRequest
    {
        Boolean active;

        public ActiveUserRequest(Boolean active)
        {
            Active = active;
        }

        public ActiveUserRequest() { }
        public bool Active { get => active; set => active = value; }

    }
}
