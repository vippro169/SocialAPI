using SocialNetwork.Application.Users.RequestModel;
using SocialNetwork.Application.Users.ResultModel;
using SocialNetwork.Domain;

namespace SocialNetwork.Application.Users
{
    public interface IUserHandler
    {
        string SignUp(SignUpRequest request);
        SignInResult SignIn(SignInRequest request);
        string GetUserName(string userId);
        GetUserResult GetUser(string userId, string authId);
        void EditUser(string userId, EditUserRequest userEdit,string authId);
    }
}
