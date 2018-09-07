using SocialNetwork.Application.Comments.RequestModel;
using SocialNetwork.Domain;
using System.Collections.Generic;

namespace SocialNetwork.Application.Comments
{
    public interface ICommentHandler
    {
        void CreateComment(CreateCommentRequest request);
        List<Comment> GetComments(string postId);
        void EditComment(string commentId, string content);
        void DeleteComment(string commentId);
    }
}
