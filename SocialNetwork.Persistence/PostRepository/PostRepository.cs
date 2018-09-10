using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using SocialNetwork.Domain;
using SocialNetwork.Persistence.MySql.ApplicationDbContext;

namespace SocialNetwork.Persistence.MySql.PostRepository
{
    public class PostRepository : IPostRepository
    {
        private readonly IApplicationDbContext _db;

        public PostRepository(IApplicationDbContext db)
        {
            _db = db;
        }

        public void CreatePost(Post post)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"INSERT INTO posts (Id, UserId, Content, Privacy, CreatedDate) " +
                              $"VALUES (@id , @userId , @content , @privacy, @createdDate);";
            cmd.Parameters.AddWithValue("@id", post.Id);
            cmd.Parameters.AddWithValue("@userId", post.UserId);
            cmd.Parameters.AddWithValue("@content", post.Content);
            cmd.Parameters.AddWithValue("@privacy", post.Privacy);
            cmd.Parameters.AddWithValue("@createdDate", post.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss"));
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public List<Post> GetAllUserPosts(string userId)
        {
            _db.Connection.Open();
            List<Post> posts = new List<Post>();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT * " +
                              $"FROM posts WHERE UserId = '{userId}' " +
                              $"ORDER BY CreatedDate DESC;";
            var reader = cmd.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    posts.Add(new Post()
                    {
                        Id = reader["Id"].ToString(),
                        UserId = reader["UserId"].ToString(),
                        Content = reader["Content"].ToString(),
                        Privacy = reader["Privacy"].ToString(),
                        CreatedDate = (DateTime)reader["CreatedDate"]
                    });
                };
            }
            _db.Connection.Close();
            return posts;
        }

        public List<Post> GetPublicUserPosts(string userId)
        {
            _db.Connection.Open();
            List<Post> posts = new List<Post>();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT * " +
                              $"FROM posts WHERE UserId = '{userId}' AND Privacy = 'Public' " +
                              $"ORDER BY CreatedDate DESC;";
            var reader = cmd.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    posts.Add(new Post()
                    {
                        Id = reader["Id"].ToString(),
                        UserId = reader["UserId"].ToString(),
                        Content = reader["Content"].ToString(),
                        Privacy = reader["Privacy"].ToString(),
                        CreatedDate = (DateTime)reader["CreatedDate"]
                    });
                };
            }
            _db.Connection.Close();
            return posts;
        }

        public List<Post> GetPublicFriendsUserPosts(string userId)
        {
            _db.Connection.Open();
            List<Post> posts = new List<Post>();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT * " +
                              $"FROM posts WHERE UserId = '{userId}' AND (Privacy = 'Public' OR Privacy = 'Friends') " +
                              $"ORDER BY CreatedDate DESC;";
            var reader = cmd.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    posts.Add(new Post()
                    {
                        Id = reader["Id"].ToString(),
                        UserId = reader["UserId"].ToString(),
                        Content = reader["Content"].ToString(),
                        Privacy = reader["Privacy"].ToString(),
                        CreatedDate = (DateTime)reader["CreatedDate"]
                    });
                };
            }
            _db.Connection.Close();
            return posts;
        }

        public void Edit(Post post)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"UPDATE posts " +
                              $"SET Content=@content, Privacy='{post.Privacy}' " +
                              $"WHERE Id='{post.Id}';";
            cmd.Parameters.AddWithValue("@content", post.Content);
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public void Delete(string postId)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"DELETE FROM posts " +
                              $"WHERE Id='{postId}';";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }


    }
}
