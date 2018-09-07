using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.Service.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string AuthUserId
        {
            get
            {
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null) throw new ApplicationException("Current user not found");
                return idClaim.Value;
            }
        }

        protected string AuthUserPath
        {
            get
            {
                var pathClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Actor);
                if (pathClaim == null) throw new ApplicationException("Current user not found");
                return pathClaim.Value;
            }
        }
    }
}
