using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public interface ITokenService
    {
        string GetToken(string username, Guid id, string role);

        string GetRefreshToken(Guid id);

        public CookieOptions GetCookieOptionsForRefreshToken();
    }
}
