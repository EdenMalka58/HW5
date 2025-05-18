using System;
using System.Security.Cryptography.Xml;

namespace HW5.Models
{
    public class ListMoviesRequest
    {
        string? title;
        DateTime? from;
        DateTime? to;
        int page = 1;
        int pageSize = Movie.DEFAULT_PAGE_SIZE;

        public ListMoviesRequest(string? title, DateTime? from, DateTime? to, int page, int pageSize)
        {
            Title = title;
            From = from;
            To = to;
            Page = page;
            PageSize = pageSize;
        }

        public ListMoviesRequest() { }

        public string? Title { get => title; set => title = value; }
        public DateTime? From { get => from; set => from = value; }
        public DateTime? To { get => to; set => to = value; }
        public int Page { get => page; set => page = value; }
        public int PageSize { get => pageSize; set => pageSize = value; }
    }
}
