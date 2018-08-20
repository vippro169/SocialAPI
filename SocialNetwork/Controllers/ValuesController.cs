using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Service.Controllers;

namespace SocialNetwork.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ValuesController : BaseController
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            return AuthId;
        }
    }
}
