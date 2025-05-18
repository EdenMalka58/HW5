using System;
using System.Security.Cryptography.Xml;

namespace HW5.Models
{
    public class RentedMovie : Movie
    {
        int rentedId;
        DateTime rentStart;
        DateTime rentEnd;

        public int RentedId { get => rentedId; set => rentedId = value; }
        public DateTime RentStart { get => rentStart; set => rentStart = value; }
        public DateTime RentEnd { get => rentEnd; set => rentEnd = value; }
    }
}
