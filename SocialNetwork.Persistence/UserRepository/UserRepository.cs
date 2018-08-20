using MySql.Data.MySqlClient;
using SocialNetwork.Domain;
using SocialNetwork.Persistence.MySql.ApplicationDbContext;
using System;
using System.Threading.Tasks;

namespace SocialNetwork.Persistence.MySql.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly IApplicationDbContext _db;

        public UserRepository(IApplicationDbContext db)
        {
            _db = db;
            _db.Connection.OpenAsync();
        }

        public void CreatUser(User user)
        {
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"INSERT INTO `users` (`Id`, `Name`, `Gender`, `Birthday`, `Email`, `PasswordHash`, `CreatedDate`) " +
                              $"VALUES ('{user.Id}', '{user.Name}', '{user.Gender}' , '{user.Birthday.ToString("yyyy-MM-dd")}', " +
                              $"'{user.Email}', '{user.PasswordHash}', '{user.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss")}');";
            cmd.ExecuteNonQuery();
        }

        public string GetPasswordHash(string email)
        {
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT PasswordHash FROM users WHERE Email = '{email}';";
            var reader = cmd.ExecuteReader();
            var result = "";
            using (reader)
            {
                while (reader.Read())
                {
                    result = reader["PasswordHash"].ToString();
                }
            }
            return result;
        }

        public string GetUserIdByEmail(string email)
        {
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT Id FROM users WHERE Email = '{email}';";
            var reader = cmd.ExecuteReader();
            string result = "";
            using (reader)
            {
                while (reader.Read())
                {
                    result = reader["Id"].ToString();
                }
            }
            return result;
        }

        public string GetUserNameById(string id)
        {
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT Name FROM users WHERE Id = '{id}';";
            var reader = cmd.ExecuteReader();
            string result = "";
            using (reader)
            {
                while (reader.Read())
                {
                    result = reader["Name"].ToString();
                }
            }
            return result;
        }

        public User GetUserById(string id)
        {
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT Id, Name, Gender, Birthday, Email, EmailPrivacy, BirthDatePrivacy, BirthYearPrivacy " +
                              $"FROM users WHERE Id = '{id}';";
            var reader = cmd.ExecuteReader();
            var user = new User();
            using (reader)
            {
                while (reader.Read())
                {
                    user.Id = reader["Id"].ToString();
                    user.Name = reader["Name"].ToString();
                    user.Gender = reader["Gender"].ToString();
                    user.Birthday = (DateTime)reader["Birthday"];
                    user.Email = reader["Email"].ToString();
                    user.EmailPrivacy = reader["EmailPrivacy"].ToString();
                    user.BirthDatePrivacy = reader["BirthDatePrivacy"].ToString();
                    user.BirthYearPrivacy = reader["BirthDatePrivacy"].ToString();
                }
            }
            return user;
        }

        public void UpdateUser(User user)
        {
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"UPDATE users " +
                              $"SET Name='{user.Name}', Gender='{user.Gender}', Birthday='{user.Birthday.ToString("yyyy-MM-dd")}', " +
                              $"EmailPrivacy='{user.EmailPrivacy}', BirthDatePrivacy='{user.BirthDatePrivacy}', BirthYearPrivacy='{user.BirthYearPrivacy}' " +
                              $"WHERE Id='{user.Id}';";
            cmd.ExecuteNonQuery();
        }
    }
}
