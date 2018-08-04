using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Application.Authentication
{
    public interface IJwtTokenOptions
    {
        string Issuer { get; set; }
        string Audience { get; set; }
        string SecurityKey { get; set; }
        int ExpireHours { get; set; }
    }
}
