using System;
using System.Security.Cryptography.Xml;

namespace HW5.Models
{
    public class ListMoviesResponse
    {
        List<Movie> movies;
        int totalRows;
        int page;
        int pageSize;
        bool hasMore;
        int fromRow;
        int toRow;

        public List<Movie> Movies { get => movies; set => movies = value; }
        public int TotalRows { get => totalRows; set => totalRows = value; }
        public int Page { get => page; set => page = value; }
        public int PageSize { get => pageSize; set => pageSize = value; }
        public bool HasMore { get => hasMore; set => hasMore = value; }
        public int FromRow { get => fromRow; set => fromRow = value; }
        public int ToRow { get => toRow; set => toRow = value; }
    }
}
