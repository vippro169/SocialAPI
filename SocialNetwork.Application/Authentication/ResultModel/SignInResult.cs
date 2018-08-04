using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Application.Authentication.ResultModel
{
    public class SignInResult
    {
        public SignInResult(string token)
        {
            JwtToken = token;
        }

        public string JwtToken { get; set; }
    }
}
