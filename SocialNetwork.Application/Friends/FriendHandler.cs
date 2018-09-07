using SocialNetwork.Application.Friends.RequestModel;
using SocialNetwork.Application.Friends.ResultModel;
using SocialNetwork.Domain;
using SocialNetwork.Persistence.MySql.FriendRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork.Application.Friends
{
    public class FriendHandler : IFriendHandler
    {
        private readonly IFriendRepository _friendRepository;

        public FriendHandler(IFriendRepository friendRepository)
        {
            _friendRepository = friendRepository;
        }

        public bool CheckFriendship(string userId, string otherId)
        {
            return _friendRepository.CheckFriendship(userId, otherId);
        }

        public void CreateFriendRequest(ClientFriendRequest request)
        {
            if (_friendRepository.GetPendingFriendRequest(request.SenderId, request.ReceiverId).Id != null) throw new ApplicationException("Friend request existed!");
            else if (CheckFriendship(request.SenderId, request.ReceiverId)) throw new ApplicationException("Already a friend!");
            else
            {
                var friendRequest = new FriendRequest()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    SenderId = request.SenderId,
                    ReceiverId = request.ReceiverId
                };
                _friendRepository.CreateFriendRequest(friendRequest);
            }
        }

        public FriendRequest GetPendingFriendRequest(string userId, string otherId)
        {
            var friendRequest = _friendRepository.GetPendingFriendRequest(userId, otherId);
            if (friendRequest.Id != null) return friendRequest;
            return null;
        }

        public List<FriendRequest> GetListPendingFriendRequest(string userId)
        {
            return _friendRepository.GetListPendingFriendRequest(userId);
        }

        public void DeleteFriendRequest(string requestId)
        {
            _friendRepository.DeleteFriendRequest(requestId);
        }

        public void ConfirmFriendRequest(FriendRequest friendRequest)
        {
            if (friendRequest.Confirmed != null)
            {
                if ((bool)friendRequest.Confirmed)
                {
                    _friendRepository.ConfirmFriendRequest(friendRequest.Id, (bool)friendRequest.Confirmed);
                    _friendRepository.CreateFriendship(friendRequest.ReceiverId, friendRequest.SenderId);
                    _friendRepository.CompleteFriendRequest(friendRequest.Id);
                }
                else
                {
                    _friendRepository.ConfirmFriendRequest(friendRequest.Id, (bool)friendRequest.Confirmed);
                    _friendRepository.CompleteFriendRequest(friendRequest.Id);
                }
            }
            else throw new ApplicationException("Confimred cannot be null!");
        }

        public List<GetFriendResult> GetListFriend(string userId)
        {
            List<GetFriendResult> friends = new List<GetFriendResult>();
            _friendRepository.GetListFriend(userId).ForEach(x =>
            {
                friends.Add(new GetFriendResult()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Path = x.Path
                });
            });
            return friends;
        }

        public void Unfriend(string userId, string friendId)
        {
            _friendRepository.DeleteFriendship(userId, friendId);
        }
    }
}
