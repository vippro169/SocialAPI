using SocialNetwork.Application.Users.RequestModel;
using SocialNetwork.Application.Users.ResultModel;

namespace SocialNetwork.Application.Users
{
    public interface IUserHandler
    {
        string SignUp(SignUpRequest request);
        string SignIn(SignInRequest request);
        string GetUserName(string userPath);
        string GetUserNameById(string userId);
        string GetUserPath(string userId);
        string GetUserId(string path);
        GetUserResult GetUser(string userId, string authId);
        void EditUser(string userId, EditUserRequest userEdit);
        string ChangePassword(string userId, ChangePasswordRequest request);
        void DeleteUser(string userId);
    }
}
