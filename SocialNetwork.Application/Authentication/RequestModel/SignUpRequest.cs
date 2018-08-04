using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialNetwork.Application.Authentication.RequestModel
{
    public class SignUpRequest
    {
        [EmailAddress(ErrorMessage = "Invalid email!")]
        [Required(ErrorMessage = "Email required!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required!")]
        public string Password { get; set; }
    }
}
