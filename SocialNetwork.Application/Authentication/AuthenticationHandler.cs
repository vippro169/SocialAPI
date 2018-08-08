using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Application.Authentication.RequestModel;
using SocialNetwork.Application.Authentication.ResultModel;
using SocialNetwork.Domain;
using SocialNetwork.Persistence.MySql.Authentication;

namespace SocialNetwork.Application.Authentication
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IJwtTokenOptions _jwtOptions;

        public AuthenticationHandler(IAuthenticationRepository authenticationRepository, IJwtTokenOptions jwtOptions)
        {
            _authenticationRepository = authenticationRepository;
            _jwtOptions = jwtOptions;
        }

        public async Task SignUpAsync(SignUpRequest request)
        {
            var account = new Account(request.Email, CreatePasswordHash(request.Password));
            await _authenticationRepository.CreatAccountAsync(account);
        }

        public async Task<SignInResult> SignInAsync(SignInRequest request)
        {
            var passwordHash = await _authenticationRepository.GetPasswordHashAsync(request.Email);
            if (ValidatePassword(request.Password, passwordHash)) return new SignInResult(CreateJwtToken(request.Email));
            else throw new ApplicationException("Invalid email or password!");
        }

        private string CreatePasswordHash(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            var bytes = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);

            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(bytes) }";
        }

        private bool ValidatePassword(string password, string hash)
        {
            if (hash == "") return false;
            var parts = hash.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var bytes = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);

            return parts[1].Equals(Convert.ToBase64String(bytes));
        }

        private string CreateJwtToken(string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_jwtOptions.ExpireHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
