using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Domain
{
    public class Comment : IEntity
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string PostId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
