using HW5.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HW5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        // GET: api/<MoviesController>
        [HttpGet]
        public ListMoviesResponse Get([FromQuery] ListMoviesRequest requestMovies)
        {
            ListMoviesResponse listMoviesResponse = new ListMoviesResponse();
            if (requestMovies.Page <= 0) requestMovies.Page = 1;
            if (requestMovies.PageSize <= 0) requestMovies.PageSize = Movie.DEFAULT_PAGE_SIZE;
            listMoviesResponse.Movies = Movie.Read(out int TotalRows, out bool hasMore, out int fromRow, out int toRow, requestMovies);
            listMoviesResponse.TotalRows = TotalRows;
            listMoviesResponse.Page = requestMovies.Page;
            listMoviesResponse.PageSize = requestMovies.PageSize;
            listMoviesResponse.HasMore = hasMore;
            listMoviesResponse.FromRow = fromRow;
            listMoviesResponse.ToRow = toRow;
            return listMoviesResponse;
        }

        // GET: api/<MoviesController>
        [HttpGet("user/{userId}")]
        public IEnumerable<RentedMovie> GetRentedMovies(int userId)
        {
            return Movie.GetRentedMovies(userId);
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("search")]
        public IEnumerable<Movie> GetByTitle(string title)
        {
            return Movie.GetByTitle(title);
        }

        [HttpGet("from/{startDate}/until/{endDate}")]
        public IEnumerable<Movie> GetByReleaseDate(DateTime startDate, DateTime endDate)
        {
            return Movie.GetByReleaseDate(startDate, endDate);
        }

        // POST api/<MoviesController>
        [HttpPost]
        public int Post([FromBody] Movie movie)
        {
            return movie.addNewMovie() ? 1 : 0; // 1 = success, 0 = error
        }

        // POST api/<MoviesController>/batch
        [HttpPost("addMovies")]
        public int Post([FromBody] List<Movie> movies)
        {
            int successCount = 0;

            foreach (var movie in movies)
            {
                if (movie.addNewMovie())
                {
                    successCount++;
                }
            }
            return successCount;
        }

        [HttpPost("rent/{movieId}")]
        public int RentMovie(int movieId, [FromBody] RentMovieRequest request)
        {
            return Movie.RentMovie(movieId, request.UserId, request.RentStart, request.RentEnd, request.TotalPrice) ? 1 : 0; // 1 = success, 0 = error
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("rent/{id}")]
        public int DeleteRentedMovie(int id)
        {
            return Movie.RemoveRentedMovie(id) ? 1 : 0; // 1 = success, 0 = error
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] Movie movie)
        {
            return movie.Update(id) ? 1 : 0; // 1 = success, 0 = error
        }

        [HttpPut("passRented/{movieId}")]
        public int PassRentedMovie(int movieId, [FromBody] PassMovieRequest request)
        {
            return Movie.PassRentedMovie(movieId, request.FromUserId, request.ToUserId) ? 1 : 0; // 1 = success, 0 = error
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            return Movie.Remove(id) ? 1 : 0; // 1 = success, 0 = error
        }

        // Return a list of all used languages in movies (for language selector in the client)
        [HttpGet("languages")]
        public IEnumerable<String> GetLanguages()
        {
            return Movie.GetLanguages();
        }

        // Return a list of all used genres in movies (for genres selector in the client)
        [HttpGet("genres")]
        public IEnumerable<String> GetGenres()
        {
            return Movie.GetGenres();
        }
    }
}