using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialNetwork.Application.Users.RequestModel
{
    public class SignInRequest
    {
        [Required(ErrorMessage = "Email required!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required!")]
        public string Password { get; set; }
    }
}
