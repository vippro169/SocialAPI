using SocialNetwork.Domain;
using System.Collections.Generic;

namespace SocialNetwork.Persistence.MySql.UserRepository
{
    public interface IUserRepository
    {
        void CreatUser(User account);
        void CreateUserRole(string id);
        void CreateAdminRole(string id);
        void UpdateRole(string id, int role);
        bool IsAdmin(string id);
        string GetPasswordHash(string email);
        string GetPasswordHashById(string userId);
        bool CheckEmailExists(string email);
        bool CheckPathExists(string path);
        string GetUserIdByEmail(string email);
        string GetUserPathByEmail(string email);
        string GetUserNameByPath(string id);
        string GetUserPathById(string id);
        string GetUserNameById(string id);
        string GetUserIdByPath(string path);
        User GetUserByPath(string path);
        List<User> SearchUserByName(string keyword);
        void UpdateUser(User user);
        void ChangePassword(string userId, string passwordHash);
        void DeleteUser(string id);
    }
}
