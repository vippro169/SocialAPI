using SocialNetwork.Domain;
using System.Collections.Generic;

namespace SocialNetwork.Persistence.MySql.FriendRepository
{
    public interface IFriendRepository
    {
        bool CheckFriendship(string userId, string otherId);
        void CreateFriendRequest(FriendRequest friendRequest);
        FriendRequest GetPendingFriendRequest(string userId, string otherId);
        List<FriendRequest> GetListPendingFriendRequest(string userId);
        void ConfirmFriendRequest(string requestId, bool confirmed);
        void CompleteFriendRequest(string requestId);
        void DeleteFriendRequest(string requestId);
        void CreateFriendship(string userId, string friendId);
        void DeleteFriendship(string userId, string friendId);
        List<User> GetListFriend(string userId);
    }
}
