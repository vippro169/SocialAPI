using SocialNetwork.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Application.Users.RequestModel
{
    public class SignUpRequest
    {
        [Required(ErrorMessage = "Name required!")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email!")]
        [Required(ErrorMessage = "Email required!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required!")]
        public string Password { get; set; }

        public string Gender { get; set; }

        [Required(ErrorMessage = "Birthday required!")]
        public DateTime Birthday { get; set; }
    }
}
