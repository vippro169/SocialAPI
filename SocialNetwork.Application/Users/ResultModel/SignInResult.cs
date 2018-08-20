using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Application.Users.ResultModel
{
    public class SignInResult
    {
        public SignInResult(string token, string id)
        {
            JwtToken = token;
            Id = id;
        }

        public string JwtToken { get; set; }

        public string Id { get; set; }
    }
}
