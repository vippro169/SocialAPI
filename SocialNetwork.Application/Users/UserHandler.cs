using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Application.Jwt;
using SocialNetwork.Application.Users.RequestModel;
using SocialNetwork.Application.Users.ResultModel;
using SocialNetwork.Domain;
using SocialNetwork.Persistence.MySql.UserRepository;

namespace SocialNetwork.Application.Users
{
    public class UserHandler : IUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenOptions _jwtOptions;

        public UserHandler(IUserRepository userRepository, IJwtTokenOptions jwtOptions)
        {
            _userRepository = userRepository;
            _jwtOptions = jwtOptions;
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

        private string CreateJwtToken(string id)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, id)
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

        public string SignUp(SignUpRequest request)
        {
            string result = null;
            string id = _userRepository.GetUserIdByEmail(request.Email);
            if (id == "")
            {
                var user = new User()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = request.Name,
                    Gender = request.Gender,
                    Birthday = request.Birthday,
                    Email = request.Email,
                    PasswordHash = CreatePasswordHash(request.Password)
                };
                _userRepository.CreatUser(user);
            }
            else result = "*Email already exists!";
            return result;
        }

        public SignInResult SignIn(SignInRequest request)
        {
            string passwordHash = _userRepository.GetPasswordHash(request.Email);
            string jwtToken = "Unauthorized";
            string userId = "";
            if (ValidatePassword(request.Password, passwordHash))
            {
                userId = _userRepository.GetUserIdByEmail(request.Email);
                jwtToken = CreateJwtToken(userId);
            }
            return new SignInResult(jwtToken, userId);
        }

        public string GetUserName(string userId)
        {
            return _userRepository.GetUserNameById(userId);
        }

        public GetUserResult GetUser(string userId, string authId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user.Id == authId) return new GetUserResult()
            {
                Id = user.Id,
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Email = user.Email,
                EmailPrivacy = user.EmailPrivacy,
                BirthDatePrivacy = user.BirthDatePrivacy,
                BirthYearPrivacy = user.BirthYearPrivacy
            };
            else return new GetUserResult()
            {
                Id = user.Id,
                Name = user.Name,
                Gender = user.Gender
            };
        }

        public void EditUser(string userId, EditUserRequest userEdit, string authId)
        {
            if (userEdit.Id != authId) throw new ApplicationException("Unauthorized!");
            if (userEdit.Id != userId) throw new ApplicationException("UserId mismatch!");
            var user = new User()
            {
                Id = userEdit.Id,
                Name = userEdit.Name,
                Gender = userEdit.Gender,
                Birthday =  userEdit.Birthday,
                EmailPrivacy = userEdit.EmailPrivacy,
                BirthDatePrivacy = userEdit.BirthDatePrivacy,
                BirthYearPrivacy = userEdit.BirthYearPrivacy
            };
            _userRepository.UpdateUser(user);
        }
    }
}
