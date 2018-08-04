using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Application.Authentication;
using SocialNetwork.Application.Authentication.RequestModel;

namespace SocialNetwork.Service.Controllers
{
    [Route("api")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationHandler _authenticationHandler;

        public AuthenticationController(IAuthenticationHandler authenticationHandler)
        {
            _authenticationHandler = authenticationHandler;
        }

        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody]SignUpRequest request)
        {
            await _authenticationHandler.SignUpAsync(request);
            return Ok();
        }

        [Route("signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody]SignInRequest request)
        {
            var result = await _authenticationHandler.SignInAsync(request);
            return Ok(result);
        }
    }
}