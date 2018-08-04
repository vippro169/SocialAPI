using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialNetwork.Domain
{
    public class Account : IEntity
    {
        public Account(string email, string passwordHash)
        {
            Id = Guid.NewGuid().ToString("N");
            Email = email;
            PasswordHash = passwordHash;
        }

        public string Id { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email!")]
        [Required(ErrorMessage = "Email required!")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "PasswordHash required!")]
        public string PasswordHash { get; set; }
    }
}
