using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Domain
{
    public class FriendRequest : IEntity
    {
        public string Id { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public bool? Confirmed { get; set; } = null;

        public bool IsDone { get; set; } = false;
    }
}
