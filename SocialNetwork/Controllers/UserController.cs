using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Users;
using SocialNetwork.Application.Users.RequestModel;

namespace SocialNetwork.Service.Controllers
{
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IUserHandler _userHandler;

        public UserController(IUserHandler userHandler)
        {
            _userHandler = userHandler;
        }

        [Route("signup")]
        [HttpPost]
        public IActionResult SignUp([FromBody]SignUpRequest request)
        {
            var result = _userHandler.SignUp(request);
            return Ok(result);
        }

        [Route("signin")]
        [HttpPost]
        public IActionResult SignIn([FromBody]SignInRequest request)
        {
            var result = _userHandler.SignIn(request);
            return Ok(result);
        }

        [Authorize]
        [Route("get-name/{userId}")]
        [HttpGet]
        public IActionResult GetUserName(string userId)
        {
            var result = _userHandler.GetUserName(userId);
            return Ok(result);
        }

        [Authorize]
        [Route("get-info/{userId}")]
        [HttpGet]
        public IActionResult GetUser(string userId)
        {
            var result = _userHandler.GetUser(userId, AuthId);
            return Ok(result);
        }

        [Authorize]
        [Route("edit/{userId}")]
        [HttpPut]
        public IActionResult EditUser(string userId, [FromBody]EditUserRequest userEdit)
        {
            _userHandler.EditUser(userId, userEdit, AuthId);
            return Ok();
        }
    }
}