using MySql.Data.MySqlClient;
using SocialNetwork.Domain;
using SocialNetwork.Persistence.MySql.ApplicationDbContext;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Persistence.MySql.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly IApplicationDbContext _db;

        public UserRepository(IApplicationDbContext db)
        {
            _db = db;
        }

        public void CreatUser(User user)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"INSERT INTO users (Id, Name, Gender, Birthday, Email, PasswordHash, Path, CreatedDate) " +
                              $"VALUES ('{user.Id}', @name, '{user.Gender}' , '{user.Birthday.ToString("yyyy-MM-dd")}', " +
                              $"'{user.Email}', '{user.PasswordHash}', '{user.Path}', '{user.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss")}');";
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public void CreateUserRole(string id)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"INSERT INTO userrole (UserId, RoleId) " +
                              $"VALUES ('{id}', 2); ";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public void CreateAdminRole(string id)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"INSERT INTO userrole (UserId, RoleId) " +
                              $"VALUES ('{id}', 1); ";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }


        public void UpdateRole(string id, int role)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"UPDATE userrole " +
                              $"SET RoleId={role}, " +
                              $"WHERE Id='{id}';";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public string GetPasswordHash(string email)
        {
            _db.Connection.Open();
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
            _db.Connection.Close();
            return result;
        }

        public string GetUserIdByEmail(string email)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT Id FROM users WHERE Email = @email;";
            cmd.Parameters.AddWithValue("@email", email);
            var reader = cmd.ExecuteReader();
            string result = "";
            using (reader)
            {
                while (reader.Read())
                {
                    result = reader["Id"].ToString();
                }
            }
            _db.Connection.Close();
            return result;
        }

        public string GetUserPathByEmail(string email)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT Path FROM users WHERE Email = @email;";
            cmd.Parameters.AddWithValue("@email", email);
            var reader = cmd.ExecuteReader();
            string result = "";
            using (reader)
            {
                while (reader.Read())
                {
                    result = reader["Path"].ToString();
                }
            }
            _db.Connection.Close();
            return result;
        }


        public string GetUserNameByPath(string path)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT Name FROM users WHERE Path = '{path}';";
            var reader = cmd.ExecuteReader();
            string result = "";
            using (reader)
            {
                while (reader.Read())
                {
                    result = reader["Name"].ToString();
                }
            }
            _db.Connection.Close();
            return result;
        }

        public string GetUserPathById(string id)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT Path FROM users WHERE Id = '{id}';";
            var reader = cmd.ExecuteReader();
            string result = "";
            using (reader)
            {
                while (reader.Read())
                {
                    result = reader["Path"].ToString();
                }
            }
            _db.Connection.Close();
            return result;
        }

        public string GetUserNameById(string id)
        {
            _db.Connection.Open();
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
            _db.Connection.Close();
            return result;
        }

        public string GetUserIdByPath(string path)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT Id FROM users WHERE Path = '{path}';";
            var reader = cmd.ExecuteReader();
            string result = "";
            using (reader)
            {
                while (reader.Read())
                {
                    result = reader["Id"].ToString();
                }
            }
            _db.Connection.Close();
            return result;
        }

        public User GetUserByPath(string path)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT Id, Name, Path, Gender, Birthday, Email, EmailPrivacy, BirthdayPrivacy " +
                              $"FROM users WHERE Path = '{path}';";
            var reader = cmd.ExecuteReader();
            var user = new User();
            using (reader)
            {
                while (reader.Read())
                {
                    user.Id = reader["Id"].ToString();
                    user.Name = reader["Name"].ToString();
                    user.Path = reader["Path"].ToString();
                    user.Gender = reader["Gender"].ToString();
                    user.Birthday = (DateTime)reader["Birthday"];
                    user.Email = reader["Email"].ToString();
                    user.EmailPrivacy = reader["EmailPrivacy"].ToString();
                    user.BirthdayPrivacy = reader["BirthdayPrivacy"].ToString();
                }
            }
            _db.Connection.Close();
            return user;
        }

        public List<User> SearchUserByName(string keyword)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT Id, Name, Path " +
                              $"FROM users WHERE Name LIKE @keyword;";
            cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
            var reader = cmd.ExecuteReader();
            List<User> users = new List<User>();
            using (reader)
            {
                while (reader.Read())
                {
                    users.Add(new User()
                    {
                        Id = reader["Id"].ToString(),
                        Name = reader["Name"].ToString(),
                        Path = reader["Path"].ToString(),
                    });
                };
            }
            _db.Connection.Close();
            return users;
        }


        public void UpdateUser(User user)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"UPDATE users " +
                              $"SET Name=@name, Gender='{user.Gender}', Birthday='{user.Birthday.ToString("yyyy-MM-dd")}', " +
                              $"EmailPrivacy='{user.EmailPrivacy}', BirthdayPrivacy='{user.BirthdayPrivacy}' " +
                              $"WHERE Id='{user.Id}';";

            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public bool CheckEmailExists(string email)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT (CASE WHEN EXISTS " +
                              $"(SELECT Name from users where Email = '{email}') then 1 else 0 end) as EmailCheck;";
            var reader = cmd.ExecuteReader();
            bool result = true;
            using (reader)
            {
                while (reader.Read())
                {
                    result = reader["EmailCheck"].ToString() == "1" ? true : false;
                }
            }
            _db.Connection.Close();
            return result;
        }

        public bool CheckPathExists(string path)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT (CASE WHEN EXISTS " +
                              $"(SELECT Name FROM users WHERE Path = '{path}') THEN 1 ELSE 0 end) as PathCheck;";
            var reader = cmd.ExecuteReader();
            bool result = true;
            using (reader)
            {
                while (reader.Read())
                {
                    result = reader["PathCheck"].ToString() == "1" ? true : false;
                }
            }
            _db.Connection.Close();
            return result;
        }

        public string GetPasswordHashById(string userId)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT PasswordHash FROM users WHERE Id = '{userId}';";
            var reader = cmd.ExecuteReader();
            var result = "";
            using (reader)
            {
                while (reader.Read())
                {
                    result = reader["PasswordHash"].ToString();
                }
            }
            _db.Connection.Close();
            return result;
        }

        public void ChangePassword(string userId, string passwordHash)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"UPDATE users " +
                              $"SET PasswordHash='{passwordHash}' " +
                              $"WHERE Id='{userId}';";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public void DeleteUser(string id)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"DELETE FROM users " +
                              $"WHERE Id='{id}';";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public bool IsAdmin(string id)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT (CASE WHEN EXISTS " +
                              $"(SELECT * FROM userrole WHERE UserId='{id}' AND RoleId=1) THEN 1 ELSE 0 end) as PathCheck;";
            var reader = cmd.ExecuteReader();
            bool result = true;
            using (reader)
            {
                while (reader.Read())
                {
                    result = reader["AdminCheck"].ToString() == "1" ? true : false;
                }
            }
            _db.Connection.Close();
            return result;
        }
    }
}
