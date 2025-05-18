using System;
using System.Security.Cryptography.Xml;

namespace HW5.Models
{
    public class RentMovieRequest
    {
        int userId;
        DateTime rentStart;
        DateTime rentEnd;
        float totalPrice;

        public int UserId { get => userId; set => userId = value; }
        public DateTime RentStart { get => rentStart; set => rentStart = value; }
        public DateTime RentEnd { get => rentEnd; set => rentEnd = value; }
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }
    }
}
