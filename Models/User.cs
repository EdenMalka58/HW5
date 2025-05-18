using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace HW5.Models
{
    public class User
    {
        int id;
        string name;
        string email;
        string password;
        Boolean active = true;
        Boolean admin = false;

        public User(string name, string email, string password, bool active, bool admin)
        {
            Name = name;
            Email = email;
            Password = password;
            Active = active;
            Admin = admin;
        }
        public User() { }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public bool Active { get => active; set => active = value; }
        public bool Admin { get => admin; set => admin = value; }

        public static List<User> Read(bool includeInActive)
        {
            DBservices dbs = new DBservices();
            return dbs.SelectUsers(includeInActive);
        }

        // encrypt the password using MD5 algorithm
        static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return password;
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // convert to hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (var b in hashBytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }

        public static User Login(string email, string password) 
        {
            DBservices dbs = new DBservices();
            string hashedPassword = HashPassword(password);
            return dbs.LogInUser(email, hashedPassword);
        }

        public int Register() {

            DBservices dbs = new DBservices();
            try
            {
                return dbs.InsertUser(this) > 0 ? 1 : 0;
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                // 2627: Violation of PRIMARY KEY constraint
                // 2601: Cannot insert duplicate key row in object with UNIQUE index
                // Handle unique key violation specifically
                return -1; // Unique key constraint violation (email already exists)
            }
            catch
            {
                return 0; // General error
            }
        }

        public string getHashedPassword()
        {
            return HashPassword(this.password);
        }

        public bool Update(int id) {
            DBservices dbs = new DBservices();
            return dbs.UpdateUser(id, this) > 0;
        }

        public static bool UpdateActive(int id, bool active)
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateUserActive(id, active) > 0;
        }
        
        public static bool Remove(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteUser(id) > 0;
        }
    }
}