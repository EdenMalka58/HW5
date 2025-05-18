using System;
using System.Security.Cryptography.Xml;

namespace HW5.Models
{
    public class PassMovieRequest
    {
        int fromUserId;
        int toUserId;

        public int FromUserId { get => fromUserId; set => fromUserId = value; }
        public int ToUserId { get => toUserId; set => toUserId = value; }
    }
}
