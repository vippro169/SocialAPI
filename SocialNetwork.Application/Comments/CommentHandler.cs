using SocialNetwork.Application.Comments.RequestModel;
using SocialNetwork.Domain;
using SocialNetwork.Persistence.MySql.CommentRepository;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Application.Comments
{
    public class CommentHandler : ICommentHandler
    {
        private readonly ICommentRepository _commentRepository;

        public CommentHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public void CreateComment(CreateCommentRequest request)
        {
            var comment = new Comment()
            {
                Id = Guid.NewGuid().ToString("N"),
                UserId = request.UserId,
                PostId = request.PostId,
                Content = request.Content
            };
            _commentRepository.CreateComment(comment);
        }

        public List<Comment> GetComments(string postId)
        {
            return _commentRepository.GetListComments(postId);
        }

        public void EditComment(string commentId, string content)
        {
            _commentRepository.Edit(commentId, content);
        }

        public void DeleteComment(string commentId)
        {
            _commentRepository.Delete(commentId);
        }
    }
}
