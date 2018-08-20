using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.Service.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string AuthId
        {
            get
            {
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null) throw new ApplicationException("Current user not found");
                return idClaim.Value;
            }
        }
    }
}
