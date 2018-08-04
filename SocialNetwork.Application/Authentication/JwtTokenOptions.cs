using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Application.Authentication
{
    public class JwtTokenOptions : IJwtTokenOptions
    {
        public JwtTokenOptions(string issuer, string audience, string key, int expireHours)
        {
            Issuer = issuer;
            Audience = audience;
            SecurityKey = key;
            ExpireHours = expireHours;
        }

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecurityKey { get; set; }
        public int ExpireHours { get; set; }
    }
}
