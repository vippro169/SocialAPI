using System;
using System.Net;
using System.Collections.Generic;
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
using SocialNetwork.Persistence.MySql.FriendRepository;
using SocialNetwork.Persistence.MySql.UserRepository;

namespace SocialNetwork.Application.Users
{
    public class UserHandler : IUserHandler
    {
        private readonly IJwtTokenOptions _jwtOptions;
        private readonly IUserRepository _userRepository;
        private readonly IFriendRepository _friendRepository;

        public UserHandler(IJwtTokenOptions jwtOptions, IUserRepository userRepository, IFriendRepository friendRepository)
        {
            _jwtOptions = jwtOptions;
            _userRepository = userRepository;
            _friendRepository = friendRepository;
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

        private string CreateJwtToken(string id, string path)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, id),
                new Claim(ClaimTypes.Actor, path)
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
            if (_userRepository.CheckEmailExists(request.Email)) result = "*Email already exists!";
            else if (_userRepository.CheckPathExists(request.Path)) result = "*Path already exists!";
            else
            {
                var user = new User()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = request.Name,
                    Gender = request.Gender,
                    Birthday = request.Birthday,
                    Email = request.Email,
                    PasswordHash = CreatePasswordHash(request.Password),
                    Path = request.Path.ToLower()
                };
                _userRepository.CreatUser(user);
                _userRepository.CreateUserRole(user.Id);
            }
            return result;
        }

        public string SignIn(SignInRequest request)
        {
            string passwordHash = _userRepository.GetPasswordHash(request.Email);
            string jwtToken = "Unauthorized";
            if (ValidatePassword(request.Password, passwordHash))
            {
                var userId = _userRepository.GetUserIdByEmail(request.Email);
                var userPath = _userRepository.GetUserPathByEmail(request.Email);
                jwtToken = CreateJwtToken(userId, userPath);
            }
            return jwtToken;
        }

        public string GetUserName(string userPath)
        {
            return _userRepository.GetUserNameByPath(userPath.ToLower());
        }

        public string GetUserNameById(string userId)
        {
            return _userRepository.GetUserNameById(userId);
        }

        public string GetUserPath(string userId)
        {
            return _userRepository.GetUserPathById(userId);
        }

        public string GetUserId(string userPath)
        {
            return _userRepository.GetUserIdByPath(userPath.ToLower());
        }

        public GetUserResult GetUser(string userPath, string authId)
        {
            var user = _userRepository.GetUserByPath(userPath);
            if (user.Id == authId) return new GetUserResult()
            {
                Id = user.Id,
                Name = user.Name,
                Path = user.Path,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Email = user.Email,
                EmailPrivacy = user.EmailPrivacy,
                BirthdayPrivacy = user.BirthdayPrivacy
            };
            else if (_friendRepository.CheckFriendship(authId, user.Id)) return new GetUserResult()
            {
                Id = user.Id,
                Name = user.Name,
                Path = user.Path,
                Gender = user.Gender,
                Birthday = user.BirthdayPrivacy != "Only me" ? (DateTime?)user.Birthday: null,
                Email = user.EmailPrivacy != "Only me" ? user.Email : null,
            };
            else return new GetUserResult()
            {
                Id = user.Id,
                Name = user.Name,
                Path = user.Path,
                Gender = user.Gender
            };
        }

        public List<GetUserResult> SearchUser(string keyword)
        {
            List<GetUserResult> users = new List<GetUserResult>();
            _userRepository.SearchUserByName(WebUtility.UrlDecode(keyword)).ForEach(x =>
            {
                users.Add(new GetUserResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Path = x.Path
                });
            });
            return users;
        }

        public void EditUser(string userId, EditUserRequest userEdit)
        {
            if (userEdit.Id != userId) throw new ApplicationException("UserId mismatch!");
            var user = new User()
            {
                Id = userEdit.Id,
                Name = userEdit.Name,
                Gender = userEdit.Gender,
                Birthday = userEdit.Birthday,
                EmailPrivacy = userEdit.EmailPrivacy,
                BirthdayPrivacy = userEdit.BirthdayPrivacy
            };
            _userRepository.UpdateUser(user);
        }

        public string ChangePassword(string userId, ChangePasswordRequest request)
        {
            var passwordHash = _userRepository.GetPasswordHashById(userId);
            if (ValidatePassword(request.Password, passwordHash)) _userRepository.ChangePassword(userId, CreatePasswordHash(request.NewPassword));
            else return "Wrong password!";
            return null;
        }

        public void DeleteUser(string userId)
        {
            _userRepository.DeleteUser(userId);
        }
    }
}
