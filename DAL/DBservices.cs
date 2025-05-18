using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using HW5.Models;
using System.Xml.Linq;
using System.Reflection.PortableExecutable;
using System.Data.Common;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{
    public const string DB_NAME = "myProjDB";

    public DBservices() {}

    private SqlConnection Connect()
    {
        try
        {
            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString(DB_NAME);
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }

    private SqlCommand CreateCommandWithStoredProcedureGeneral(String spName, SqlConnection con, Dictionary<string, object> paramDic)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object
        cmd.Connection = con; // assign the connection to the command object
        cmd.CommandText = spName; // can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10; // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        if (paramDic != null)
            foreach (KeyValuePair<string, object> param in paramDic)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }

        return cmd;
    }

    private int CallStoredProcedure(string spName, Dictionary<string, object> paramDic)
    {
        SqlConnection con = Connect();
        SqlCommand cmd = CreateCommandWithStoredProcedureGeneral(spName, con, paramDic);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }
    private int CallReadStoredProcedure(string spName, Dictionary<string, object> paramDic, 
        Func<SqlDataReader, int> OnReadNextRow, Func<SqlDataReader, int> OnReadNextResultRow = null)
    {
        int result = 0;
        SqlConnection con = Connect();
        SqlCommand cmd = CreateCommandWithStoredProcedureGeneral(spName, con, paramDic);
        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        try
        {
            while (dataReader.Read())
            {
                result += OnReadNextRow(dataReader);
            }
            // If store procedure returns two datasets, read the next result from datareader
            if (OnReadNextResultRow != null)
            {
                if (dataReader.NextResult())
                {
                    while (dataReader.Read())
                    {
                        result += OnReadNextResultRow(dataReader);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
        return result;
    }

    private string GetFieldStr(SqlDataReader dataReader, string fieldName) {
        object value = dataReader[fieldName];
        if (value == null) return null;
        return value.ToString();
    }

    private int GetFieldInt(SqlDataReader dataReader, string fieldName)
    {
        object value = dataReader[fieldName];
        if (value == null) return 0;
        return Convert.ToInt32(value);
    }

    private double GetFieldDouble(SqlDataReader dataReader, string fieldName)
    {
        object value = dataReader[fieldName];
        if (value == null) return 0;
        return Convert.ToDouble(value);
    }

    private float GetFieldFloat(SqlDataReader dataReader, string fieldName)
    {
        object value = dataReader[fieldName];
        if (value == null) return 0;
        return Convert.ToSingle(value);
    }

    private DateTime GetFieldDate(SqlDataReader dataReader, string fieldName)
    {
        object value = dataReader[fieldName];
        if (value == null) return DateTime.MinValue;
        return (DateTime)value;
    }

    private Boolean GetFieldBool(SqlDataReader dataReader, string fieldName)
    {
        object value = dataReader[fieldName];
        if (value == null) return false;
        return (Boolean)value;
    }

    public int InsertUser(User user)
    {
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@name", user.Name);
        paramDic.Add("@email", user.Email);
        paramDic.Add("@password", user.getHashedPassword());
        return CallStoredProcedure("SP_InsertUser", paramDic);
    }

    public int UpdateUser(int id, User user)
    {
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", id);
        paramDic.Add("@name", user.Name);
        paramDic.Add("@email", user.Email);
        paramDic.Add("@password", user.getHashedPassword());
        return CallStoredProcedure("SP_UpdateUser", paramDic);
    }

    public int UpdateUserActive(int id, bool active)
    {
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", id);
        paramDic.Add("@active", active ? 1 : 0);
        return CallStoredProcedure("SP_UpdateUserActive", paramDic);
    }

    public int DeleteUser(int id)
    {
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", id);
        return CallStoredProcedure("SP_DeleteUser", paramDic);
    }

    public int InsertMovie(Movie movie)
    {
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@url", movie.Url);
        paramDic.Add("@primaryTitle", movie.PrimaryTitle);
        paramDic.Add("@description", movie.Description);
        paramDic.Add("@primaryImage", movie.PrimaryImage);
        paramDic.Add("@year", movie.Year);
        paramDic.Add("@releaseDate", movie.ReleaseDate);
        paramDic.Add("@language", movie.Language);
        paramDic.Add("@budget", movie.Budget);
        paramDic.Add("@genres", movie.Genres);
        paramDic.Add("@isAdult", movie.IsAdult);
        paramDic.Add("@runtimeMinutes", movie.RuntimeMinutes);
        paramDic.Add("@averageRating", movie.AverageRating);
        paramDic.Add("@numVotes", movie.NumVotes);
        return CallStoredProcedure("SP_InsertMovie", paramDic);
    }

    public int UpdateMovie(int id, Movie movie)
    {
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", id);
        paramDic.Add("@url", movie.Url);
        paramDic.Add("@primaryTitle", movie.PrimaryTitle);
        paramDic.Add("@description", movie.Description);
        paramDic.Add("@primaryImage", movie.PrimaryImage);
        paramDic.Add("@year", movie.Year);
        paramDic.Add("@releaseDate", movie.ReleaseDate);
        paramDic.Add("@language", movie.Language);
        paramDic.Add("@budget", movie.Budget);
        paramDic.Add("@genres", movie.Genres);
        paramDic.Add("@isAdult", movie.IsAdult);
        paramDic.Add("@runtimeMinutes", movie.RuntimeMinutes);
        paramDic.Add("@averageRating", movie.AverageRating);
        paramDic.Add("@numVotes", movie.NumVotes);
        return CallStoredProcedure("SP_UpdateMovie", paramDic);
    }

    public int RentMovie(int movieId, int userId, DateTime rentStart, DateTime rentEnd, float totalPrice)
    {
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userId", userId);
        paramDic.Add("@movieId", movieId);
        paramDic.Add("@rentStart", rentStart);
        paramDic.Add("@rentEnd", rentEnd);
        paramDic.Add("@totalPrice", totalPrice);
        return CallStoredProcedure("SP_RentMovie", paramDic);
    }


    public int DeleteMovie(int id)
    {
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", id);
        return CallStoredProcedure("SP_DeleteMovie", paramDic);
    }

    public int DeleteRentedMovie(int id)
    {
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", id);
        return CallStoredProcedure("SP_DeleteRentedMovie", paramDic);
    }

    public User LogInUser(string email, string password)
    {
        User user = null;
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@email", email);
        paramDic.Add("@password", password);


        CallReadStoredProcedure("SP_LogInUser", paramDic, dataReader =>
        {
            user = ReadUser(dataReader);
            return 1;
        });
        return user;
    }

    public List<User> SelectUsers(bool includeInActive)
    {
        List<User> list = new List<User>();
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@includeInActive", includeInActive ? 1 : 0);

        CallReadStoredProcedure("SP_SelectUsers", paramDic, dataReader =>
        {
            User user = ReadUser(dataReader);
            list.Add(user);
            return 1;
        });
        return list;
    }

    public List<RentedMovie> SelectRentedMovies(int userId)
    {
        List<RentedMovie> list = new List<RentedMovie>();
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userId", userId);

        CallReadStoredProcedure("SP_SelectRentedMovies", paramDic, dataReader =>
        {
            RentedMovie rentedMovie = new RentedMovie();
            rentedMovie.RentedId = GetFieldInt(dataReader, "rentedId");
            rentedMovie.RentStart = GetFieldDate(dataReader, "rentStart");
            rentedMovie.RentEnd = GetFieldDate(dataReader, "rentEnd");
            ReadMovie(dataReader, rentedMovie);
            list.Add(rentedMovie);
            return 1;
        });
        return list;
    }

    public List<Movie> SelectMovies(out int totalRows, string title, DateTime? from, DateTime? to, int fromRow, int toRow)
    {
        totalRows = 0;
        List<Movie> list = new List<Movie>();
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@title", title);
        paramDic.Add("@from", from);
        paramDic.Add("@to", to);
        paramDic.Add("@fromRow", fromRow);
        paramDic.Add("@toRow", toRow);
        int moviesTotalRows = 0;
        CallReadStoredProcedure("SP_SelectMovies", paramDic, dataReader =>
        {
            Movie movie = new Movie();
            ReadMovie(dataReader, movie);
            list.Add(movie);
            return 1;
        }, nextDataReader => {
            moviesTotalRows = GetFieldInt(nextDataReader, "totalRows");
            return 1;
        });

        totalRows = moviesTotalRows;
        return list;
    }

    public List<string> SelectLanguages()
    {
        List<String> list = new List<String>();
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        
        CallReadStoredProcedure("SP_SelectLanguages", null, dataReader =>
        {
            string language = GetFieldStr(dataReader, "language");
            list.Add(language);
            return 1;
        });
        return list;
    }

    public int PassRentedMovie(int movieId, int fromUserId, int toUserId)
    {
        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@movieId", movieId);
        paramDic.Add("@fromUserId", fromUserId);
        paramDic.Add("@toUserId", toUserId);
        return CallStoredProcedure("SP_PassRentedMovie", paramDic);
    }

    private User ReadUser(SqlDataReader dataReader)
    {
        User user = new User();
        user.Id = GetFieldInt(dataReader, "id");
        user.Name = GetFieldStr(dataReader, "name");
        user.Email = GetFieldStr(dataReader, "email");
        user.Password = "********";
        user.Active = GetFieldBool(dataReader, "active");
        user.Admin = GetFieldBool(dataReader, "admin");
        return user;
    }

    private void ReadMovie(SqlDataReader dataReader, Movie movie)
    {
        movie.Id = GetFieldInt(dataReader, "id");
        movie.Url = GetFieldStr(dataReader, "url");
        movie.PrimaryTitle = GetFieldStr(dataReader, "primaryTitle");
        movie.Description = GetFieldStr(dataReader, "description");
        movie.PrimaryImage = GetFieldStr(dataReader, "primaryImage");
        movie.Year = GetFieldInt(dataReader, "year");
        movie.ReleaseDate = GetFieldDate(dataReader, "releaseDate");
        movie.Language = GetFieldStr(dataReader, "language");
        movie.Budget = GetFieldDouble(dataReader, "budget");
        movie.GrossWorldwide = GetFieldDouble(dataReader, "grossWorldwide");
        movie.Genres = GetFieldStr(dataReader, "genres");
        movie.IsAdult = GetFieldBool(dataReader, "isAdult");
        movie.RuntimeMinutes = GetFieldInt(dataReader, "runtimeMinutes");
        movie.AverageRating = GetFieldFloat(dataReader, "averageRating");
        movie.NumVotes = GetFieldInt(dataReader, "numVotes");
        movie.PriceToRent = GetFieldInt(dataReader, "priceToRent");
    }
}