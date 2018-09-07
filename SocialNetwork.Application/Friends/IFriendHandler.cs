using SocialNetwork.Application.Friends.RequestModel;
using SocialNetwork.Application.Friends.ResultModel;
using SocialNetwork.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.Application.Friends
{
    public interface IFriendHandler
    {
        void CreateFriendRequest(ClientFriendRequest request);
        FriendRequest GetPendingFriendRequest(string userId, string otherId);
        List<FriendRequest> GetListPendingFriendRequest(string userId);
        void DeleteFriendRequest(string requestId);
        void ConfirmFriendRequest(FriendRequest friendRequest);
        bool CheckFriendship(string userId, string otherId);
        List<GetFriendResult> GetListFriend(string userId);
        void Unfriend(string userId, string friendId);
    }
}
