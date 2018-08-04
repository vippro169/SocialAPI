using SocialNetwork.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Persistence.MySql.Authentication
{
    public interface IAuthenticationRepository
    {
        Task CreatAccountAsync(Account account);
        Task<string> GetPasswordHashAsync(string email);
    }
}
