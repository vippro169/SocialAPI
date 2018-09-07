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
        [Route("get-name/{userPath}")]
        [HttpGet]
        public IActionResult GetUserName(string userPath)
        {
            var result = _userHandler.GetUserName(userPath);
            return Ok(result);
        }

        [Authorize]
        [Route("get-path/{userId}")]
        [HttpGet]
        public IActionResult GetUserPath(string userId)
        {
            var result = _userHandler.GetUserPath(userId);
            return Ok(result);
        }

        [Authorize]
        [Route("get-name-by-id/{userId}")]
        [HttpGet]
        public IActionResult GetUserNameById(string userId)
        {
            var result = _userHandler.GetUserNameById(userId);
            return Ok(result);
        }

        [Authorize]
        [Route("get-id/{userPath}")]
        [HttpGet]
        public IActionResult GetUserId(string userPath)
        {
            var result = _userHandler.GetUserId(userPath);
            return Ok(result);
        }

        [Authorize]
        [Route("get-info/{userPath}")]
        [HttpGet]
        public IActionResult GetUser(string userPath)
        {
            var result = _userHandler.GetUser(userPath, AuthUserId);
            return Ok(result);
        }

        [Authorize]
        [Route("edit/{userId}")]
        [HttpPut]
        public IActionResult EditUser(string userId, [FromBody]EditUserRequest request)
        {
            _userHandler.EditUser(userId, request);
            return Ok();
        }

        [Authorize]
        [Route("change-password/{userId}")]
        [HttpPut]
        public IActionResult ChangePassword(string userId, [FromBody]ChangePasswordRequest request)
        {
            var result = _userHandler.ChangePassword(userId, request);
            return Ok(result);
        }

        [Authorize]
        [Route("delete/{userId}")]
        [HttpDelete]
        public IActionResult DeleteUser(string userId)
        {
            _userHandler.DeleteUser(userId);
            return Ok();
        }
    }
}