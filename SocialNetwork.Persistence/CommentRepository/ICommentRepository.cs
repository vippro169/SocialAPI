using SocialNetwork.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Persistence.MySql.CommentRepository
{
    public interface ICommentRepository
    {
        void CreateComment(Comment comment);
        List<Comment> GetListComments(string postId);
        void Edit(string id, string content);
        void Delete(string id);
    }
}
