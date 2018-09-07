using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialNetwork.Application.Posts.RequestModel
{
    public class CreatePostRequest
    {
        [Required(ErrorMessage = "UserId required!")]
        public string UserId { get; set; }
      
        [Required(ErrorMessage = "Post content required!")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Post privacy required!")]
        public string Privacy { get; set; }
    }
}
