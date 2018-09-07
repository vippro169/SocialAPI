using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialNetwork.Application.Friends.RequestModel
{
    public class ClientFriendRequest
    {
        [Required(ErrorMessage = "SenderId required!")]
        public string SenderId { get; set; }

        [Required(ErrorMessage = "ReceiverId required!")]
        public string ReceiverId { get; set; }
    }
}
