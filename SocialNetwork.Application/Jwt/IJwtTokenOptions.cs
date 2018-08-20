namespace SocialNetwork.Application.Jwt
{
    public interface IJwtTokenOptions
    {
        string Issuer { get; set; }
        string Audience { get; set; }
        string SecurityKey { get; set; }
        int ExpireHours { get; set; }
    }
}
