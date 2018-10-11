using SocialNetwork.Application.Users.RequestModel;
using SocialNetwork.Application.Users.ResultModel;
using System.Collections.Generic;

namespace SocialNetwork.Application.Users
{
    public interface IUserHandler
    {
        string SignUp(int callerId, SignUpRequest request);
        string SignIn(SignInRequest request);
        string GetUserName(string userPath);
        string GetUserNameById(string userId);
        string GetUserPath(string userId);
        string GetUserId(string path);
        GetUserResult GetUser(string userId, string authId);
        List<GetUserResult> SearchUser(string keyword);
        void EditUser(string userId, EditUserRequest userEdit);
        string ChangePassword(string userId, ChangePasswordRequest request);
        void DeleteUser(string userId);
    }
}
