using MySql.Data.MySqlClient;
using SocialNetwork.Domain;
using SocialNetwork.Persistence.MySql.ApplicationDbContext;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Persistence.MySql.CommentRepository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IApplicationDbContext _db;

        public CommentRepository(IApplicationDbContext db)
        {
            _db = db;
        }

        public void CreateComment(Comment comment)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"INSERT INTO comments (Id, UserId, PostId, Content, CreatedDate) " +
                              $"VALUES ('{comment.Id}', '{comment.UserId}', '{comment.PostId}', '{comment.Content}' , '{comment.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss")}');";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public List<Comment> GetListComments(string postId)
        {
            _db.Connection.Open();
            List<Comment> comments = new List<Comment>();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"SELECT * " +
                              $"FROM comments WHERE PostId = '{postId}' " +
                              $"ORDER BY CreatedDate DESC;";
            var reader = cmd.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    comments.Add(new Comment()
                    {
                        Id = reader["Id"].ToString(),
                        UserId = reader["UserId"].ToString(),
                        PostId = reader["PostId"].ToString(),
                        Content = reader["Content"].ToString(),
                        CreatedDate = (DateTime)reader["CreatedDate"]
                    });
                };
            }
            _db.Connection.Close();
            return comments;
        }

        public void Edit(string id, string content)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"UPDATE comments " +
                              $"SET Content='{content}'" +
                              $"WHERE Id='{id}';";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }

        public void Delete(string id)
        {
            _db.Connection.Open();
            var cmd = _db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = $"DELETE FROM comments " +
                              $"WHERE Id='{id}';";
            cmd.ExecuteNonQuery();
            _db.Connection.Close();
        }
    }
}
