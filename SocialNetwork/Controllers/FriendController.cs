using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Friends;
using SocialNetwork.Application.Friends.RequestModel;
using SocialNetwork.Domain;

namespace SocialNetwork.Service.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/friend")]
    public class FriendController : BaseController
    {
        private readonly IFriendHandler _friendHandler;

        public FriendController(IFriendHandler friendHandler)
        {
            _friendHandler = friendHandler;
        }

        [Route("create-friend-request")]
        [HttpPost]
        public IActionResult CreateFriendRequest([FromBody]ClientFriendRequest request)
        {
            _friendHandler.CreateFriendRequest(request);
            return Ok();
        }

        [Route("get-friend-request/{id}")]
        [HttpGet]
        public IActionResult GetPendingFriendRequest(string id)
        {
            var result = _friendHandler.GetPendingFriendRequest(AuthUserId, id);
            return Ok(result);
        }

        [Route("get-list-friend-request/{id}")]
        [HttpGet]
        public IActionResult GetListPendingFriendRequest(string id)
        {
            var result = _friendHandler.GetListPendingFriendRequest(id);
            return Ok(result);
        }

        [Route("delete-friend-request/{requestId}")]
        [HttpDelete]
        public IActionResult DeleteFriendRequest(string requestId)
        {
            _friendHandler.DeleteFriendRequest(requestId);
            return Ok();
        }

        [Route("confirm-request/{requestId}")]
        [HttpPut]
        public IActionResult ConfirmFriendRequest(string requestId, [FromBody]FriendRequest request)
        {
            _friendHandler.ConfirmFriendRequest(request);
            return Ok();
        }

        [Route("check-friendship/{id}")]
        [HttpGet]
        public IActionResult CheckFriendship(string id)
        {
            var result = _friendHandler.CheckFriendship(AuthUserId, id);
            return Ok(result);
        }

        [Route("get-list-friend/{id}")]
        [HttpGet]
        public IActionResult GetListFriend(string id)
        {
            var result = _friendHandler.GetListFriend(id);
            return Ok(result);
        }

        [Route("unfriend/{friendId}")]
        [HttpDelete]
        public IActionResult Unfriend(string friendId)
        {
            _friendHandler.Unfriend(AuthUserId, friendId);
            return Ok();
        }
    }
}
