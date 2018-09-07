using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Application.Users.RequestModel
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Password required!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "New password required!")]
        public string NewPassword { get; set; }
    }
}
