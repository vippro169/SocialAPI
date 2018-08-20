using SocialNetwork.Domain;

namespace SocialNetwork.Persistence.MySql.UserRepository
{
    public interface IUserRepository
    {
        void CreatUser(User account);
        string GetPasswordHash(string email);
        string GetUserIdByEmail(string email);
        string GetUserNameById(string id);
        User GetUserById(string id);
        void UpdateUser(User user);
    }
}
