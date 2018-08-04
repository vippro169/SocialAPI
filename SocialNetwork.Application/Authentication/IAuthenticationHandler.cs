using SocialNetwork.Application.Authentication.RequestModel;
using SocialNetwork.Application.Authentication.ResultModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Authentication
{
    public interface IAuthenticationHandler
    {
        Task SignUpAsync(SignUpRequest request);
        Task<SignInResult> SignInAsync(SignInRequest request);
    }
}
