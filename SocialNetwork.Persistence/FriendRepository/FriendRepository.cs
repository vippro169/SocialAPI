using MySql.Data.MySqlClient;
using SocialNetwork.Domain;
using SocialNetwork.Persistence.MySql.ApplicationDbContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Persistence.MySql.FriendRepository
{
    public class FriendRepository : IFriendRepository
    {
        private readonly IApplicationDbContext _db;

        public FriendRepository(IApplicationDbContext db)
        {
            _db = db;
        }

        public bool CheckFriendship(string userId, string otherId)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT CASE WHEN EXISTS " +
                              $"(SELECT users.Id FROM (SELECT Id FROM users WHERE Id = '{otherId}') AS users INNER JOIN " +
                              $"(SELECT FriendId AS Id FROM friendship WHERE UserId='{userId}' UNION " +
                              $"SELECT UserId AS Id FROM friendship WHERE FriendId ='{userId}') AS friends ON users.Id = friends.Id) " +
                              $"THEN 1 ELSE 0 END AS FriendshipCheck";
            var reader = cmd.ExecuteReader();
            bool result = true;
            using (reader)
            {
                while (reader.Read())
                {
                    result = reader["FriendshipCheck"].ToString() == "1" ? true : false;
                }
            }
            _db.Connection.Close();
            return result;
        }

        public void CreateFriendRequest(FriendRequest friendRequest)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"INSERT INTO friendrequests (Id, SenderId, ReceiverId) " +
                              $"VALUES ('{friendRequest.Id}', '{friendRequest.SenderId}', '{friendRequest.ReceiverId}');";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public FriendRequest GetPendingFriendRequest(string userId, string otherId)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT * FROM friendrequests " +
                              $"WHERE ((SenderId = '{userId}' AND ReceiverId = '{otherId}') " +
                              $"OR (SenderId = '{otherId}' AND ReceiverId = '{userId}'))" +
                              $"AND IsDone = 0;";
            var reader = cmd.ExecuteReader();
            var friendRequest = new FriendRequest();
            using (reader)
            {
                while (reader.Read())
                {
                    friendRequest.Id = reader["Id"].ToString();
                    friendRequest.SenderId = reader["SenderId"].ToString();
                    friendRequest.ReceiverId = reader["ReceiverId"].ToString();
                }
            }
            _db.Connection.Close();
            return friendRequest;
        }

        public FriendRequest GetFriendRequestById(string requestId)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT * FROM friendrequests " +
                              $"WHERE Id=''{requestId};";
            var reader = cmd.ExecuteReader();
            var friendRequest = new FriendRequest();
            using (reader)
            {
                while (reader.Read())
                {
                    friendRequest.Id = reader["Id"].ToString();
                    friendRequest.SenderId = reader["SenderId"].ToString();
                    friendRequest.ReceiverId = reader["ReceiverId"].ToString();
                    if (reader["Confirmed"] != DBNull.Value)
                    {
                        friendRequest.Confirmed = reader["Confirmed"].ToString() == "1" ? true : false;
                    }
                }
            }
            _db.Connection.Close();
            return friendRequest;
        }

        public List<FriendRequest> GetListPendingFriendRequest(string userId)
        {
            _db.Connection.Open();
            List<FriendRequest> friendRequests = new List<FriendRequest>();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT * " +
                              $"FROM friendrequests WHERE ReceiverId='{userId}' AND Confirmed IS NULL AND IsDone = 0;";
            var reader = cmd.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    friendRequests.Add(new FriendRequest()
                    {
                        Id = reader["Id"].ToString(),
                        SenderId = reader["SenderId"].ToString(),
                        ReceiverId = reader["ReceiverId"].ToString()
                    });
                };
            }
            _db.Connection.Close();
            return friendRequests;
        }

        public void ConfirmFriendRequest(string requestId, bool confirmed)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            var intConfirmed = confirmed ? 1 : 0;
            cmd.CommandText = $"UPDATE friendrequests " +
                              $"SET Confirmed='{ intConfirmed }' " +
                              $"WHERE Id='{requestId}';";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public void CompleteFriendRequest(string requestId)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"UPDATE friendrequests " +
                              $"SET IsDone=1 " +
                              $"WHERE Id='{requestId}';";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public void DeleteFriendRequest(string requestId)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"DELETE FROM friendrequests " +
                              $"WHERE Id='{requestId}';";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public void CreateFriendship(string userId, string friendId)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"INSERT INTO friendship (UserId, FriendId) " +
                              $"VALUES ('{userId}', '{friendId}');";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public void DeleteFriendship(string userId, string friendId)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"DELETE FROM friendship " +
                              $"WHERE ( UserId='{userId}' AND FriendId='{friendId}') " +
                              $"OR ( UserId='{friendId}' AND FriendId='{userId}');";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public List<User> GetListFriend(string userId)
        {
            _db.Connection.Open();
            List<User> friendList = new List<User>();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT users.Id, Name, Path " +
                              $"FROM (SELECT FriendId AS Id FROM friendship WHERE UserId='{userId}' " +
                              $"UNION SELECT UserId AS Id FROM friendship WHERE FriendId='{userId}') AS friends " +
                              $"INNER JOIN users ON users.Id = friends.Id " +
                              $"ORDER BY Name ASC;";
            var reader = cmd.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    friendList.Add(new User()
                    {
                        Id = reader["Id"].ToString(),
                        Name = reader["Name"].ToString(),
                        Path = reader["Path"].ToString()
                    });
                };
            }
            _db.Connection.Close();
            return friendList;
        }
    }
}
