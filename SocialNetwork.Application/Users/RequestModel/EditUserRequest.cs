using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Application.Users.RequestModel
{
    public class EditUserRequest
    {
        [Required(ErrorMessage = "Id required!")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Name required!")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Gender required!")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Birthday required!")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Email Privacy required!")]
        public string EmailPrivacy { get; set; }

        [Required(ErrorMessage = "Birthday Privacy required!")]
        public string BirthdayPrivacy { get; set; }
    }
}
