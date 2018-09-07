using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Application.Comments.RequestModel
{
    public class CreateCommentRequest
    {
        [Required(ErrorMessage = "UserId required!")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "PostId required!")]
        public string PostId { get; set; }

        [Required(ErrorMessage = "Comment content required!")]
        public string Content { get; set; }
    }
}
