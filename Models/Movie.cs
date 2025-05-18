using System;

namespace HW5.Models
{
    public class Movie
    {
        public const int DEFAULT_PAGE_SIZE = 50;
        public const int MAX_ROWS_IN_PAGE = 1000000000;

        int id;
        string url;
        string primaryTitle;
        string description;
        string primaryImage;
        int year;
        DateTime releaseDate;
        string language;
        double budget;
        double grossWorldwide;
        string genres;
        Boolean isAdult;
        int runtimeMinutes;
        float averageRating;
        int numVotes;
        int priceToRent;

        public Movie(string url, string primaryTitle, string description, string primaryImage, int year, DateTime releaseDate, string language, double budget, double grossWorldwide, string genres, bool isAdult, int runtimeMinutes, float averageRating, int numVotes, int priceToRent)
        {
            Url = url;
            PrimaryTitle = primaryTitle;
            Description = description;
            PrimaryImage = primaryImage;
            Year = year;
            ReleaseDate = releaseDate;
            Language = language;
            Budget = budget;
            GrossWorldwide = grossWorldwide;
            Genres = genres;
            IsAdult = isAdult;
            RuntimeMinutes = runtimeMinutes;
            AverageRating = averageRating;
            NumVotes = numVotes;
            PriceToRent = priceToRent;
        }

        public Movie() { }

        public int Id { get => id; set => id = value; }
        public string Url { get => url; set => url = value; }
        public string PrimaryTitle { get => primaryTitle; set => primaryTitle = value; }
        public string Description { get => description; set => description = value; }
        public string PrimaryImage { get => primaryImage; set => primaryImage = value; }
        public int Year { get => year; set => year = value; }
        public DateTime ReleaseDate { get => releaseDate; set => releaseDate = value; }
        public string Language { get => language; set => language = value; }
        public double Budget { get => budget; set => budget = value; }
        public double GrossWorldwide { get => grossWorldwide; set => grossWorldwide = value; }
        public string Genres { get => genres; set => genres = value; }
        public bool IsAdult { get => isAdult; set => isAdult = value; }
        public int RuntimeMinutes { get => runtimeMinutes; set => runtimeMinutes = value; }
        public float AverageRating { get => averageRating; set => averageRating = value; }
        public int NumVotes { get => numVotes; set => numVotes = value; }
        public int PriceToRent { get => priceToRent; set => priceToRent = value; }

        public Boolean addNewMovie()
        {
            DBservices dbs = new DBservices();
            return dbs.InsertMovie(this) > 0;
        }

        public static List<Movie> Read(out int totalRows, out bool hasMore, out int fromRow, out int toRow, ListMoviesRequest requestMovies)
        {
            fromRow = ((requestMovies.Page - 1) * requestMovies.PageSize) + 1;
            toRow = (fromRow + requestMovies.PageSize) - 1;
            DBservices dbs = new DBservices();
            List<Movie> movies = dbs.SelectMovies(out totalRows, requestMovies.Title,
                requestMovies.From, requestMovies.To, fromRow, toRow);
            
            toRow = Math.Min(toRow, totalRows);
            hasMore = toRow < totalRows;
            return movies;
        }

        public static List<Movie> Read()
        {
            return Read(out int totalRows, out bool hasMore, out int fromRow, out int toRow, 
                new ListMoviesRequest(null, null, null, 1, Movie.MAX_ROWS_IN_PAGE));
        }

        public static List<Movie> GetByTitle(string title)
        {
            return Read(out int totalRows, out bool hasMore, out int fromRow, out int toRow,
                            new ListMoviesRequest(title, null, null, 1, Movie.MAX_ROWS_IN_PAGE));
        }

        public static List<Movie> GetByReleaseDate(DateTime startDate, DateTime endDate)
        {
            return Read(out int totalRows, out bool hasMore, out int fromRow, out int toRow,
                new ListMoviesRequest(null, startDate, endDate, 1, Movie.MAX_ROWS_IN_PAGE));
        }

        public static DateTime ISODateStrToDate(string ISOStr) {
            try
            {
                return DateTime.Parse(ISOStr, null, System.Globalization.DateTimeStyles.RoundtripKind);
            }
            catch {
                return DateTime.Today; 
            }
        }

        public bool Update(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateMovie(id, this) > 0;
        }
        public static bool Remove(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteMovie(id) > 0;
        }
        public static List<string> GetLanguages() {

            DBservices dbs = new DBservices();
            List<string> languages = dbs.SelectLanguages();
            return languages;
        }

        public static List<string> GetGenres()
        {
            List<string> uniqueGenres = new List<string>();
            List<Movie> movies = Read();
            foreach (Movie movie in movies)
            {
                if (!string.IsNullOrEmpty(movie.Genres))
                {
                    string[] genres = movie.Genres.Split(",");
                    foreach (string genre in genres)

                        if (!uniqueGenres.Contains(genre))
                        {
                            uniqueGenres.Add(genre);
                        }
                }
            }
            return uniqueGenres;
        }

        public static List<RentedMovie> GetRentedMovies(int userId)
        {
            DBservices dbs = new DBservices();
            return dbs.SelectRentedMovies(userId);
        }

        public static bool PassRentedMovie(int movieId, int fromUserId, int toUserId) 
        {
            DBservices dbs = new DBservices();
            return dbs.PassRentedMovie(movieId, fromUserId, toUserId) > 0;
        }

        public static bool RentMovie(int movieId, int userId, DateTime rentStart, DateTime rentEnd, float totalPrice)
        {
            DBservices dbs = new DBservices();
            return dbs.RentMovie(movieId, userId, rentStart, rentEnd, totalPrice) > 0;
        }

        public static bool RemoveRentedMovie(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteRentedMovie(id) > 0;
        }
    }
}