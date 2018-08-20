using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Application.Users.ResultModel
{
    public class GetUserResult
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string Email { get; set; }

        public string EmailPrivacy { get; set; }

        public string BirthDatePrivacy { get; set; }

        public string BirthYearPrivacy { get; set; }
    }
}
