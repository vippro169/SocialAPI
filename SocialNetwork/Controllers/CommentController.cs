using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Comments;
using SocialNetwork.Application.Comments.RequestModel;

namespace SocialNetwork.Service.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/comment")]
    public class CommentController : BaseController
    {
        private readonly ICommentHandler _commentHandler;

        public CommentController(ICommentHandler commentHandler)
        {
            _commentHandler = commentHandler;
        }

        [Route("create")]
        [HttpPost]
        public IActionResult CreateComment([FromBody]CreateCommentRequest request)
        {
            _commentHandler.CreateComment(request);
            return Ok();
        }

        [Route("get-comments/{postId}")]
        [HttpGet]
        public IActionResult GetComments(string postId)
        {
            var result = _commentHandler.GetComments(postId);
            return Ok(result);
        }

        [Route("edit/{commentId}")]
        [HttpPut]
        public IActionResult EditPost(string commentId, [FromBody]string content)
        {
            _commentHandler.EditComment(commentId, content);
            return Ok();
        }

        [Route("delete/{commentId}")]
        [HttpDelete]
        public IActionResult DeletePost(string commentId)
        {
            _commentHandler.DeleteComment(commentId);
            return Ok();
        }
    }
}