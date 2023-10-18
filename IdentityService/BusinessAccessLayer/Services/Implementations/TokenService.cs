using BAL.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BAL.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value; 
        }

        public string GetToken(string username, Guid id, string role)
        {
            var creds = GetCreds(true);
            var claims = GetClaims(username, id, role);
            var tokenDescriptor = CreateTokenDescriptor(claims, creds);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GetRefreshToken(Guid id)
        {
            var creds = GetCreds(false);
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.NameId, id.ToString())};
            var refreshtokenDescriptor = CreateRefresgTokenDescriptor(claims, creds);
            var refreshTokenHandler = new JwtSecurityTokenHandler();
            var token = refreshTokenHandler.CreateToken(refreshtokenDescriptor);

            return refreshTokenHandler.WriteToken(token);
        }
            
        private SigningCredentials GetCreds(bool type)
        {
            SigningCredentials signingCredentionals;

            if (type)
            {
                signingCredentionals = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                    SecurityAlgorithms.HmacSha256);
            }
            else
            {
                signingCredentionals = new SigningCredentials(
                     new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshSecret)),
                     SecurityAlgorithms.HmacSha256);
            }
            return signingCredentionals;
        }

        private List<Claim> GetClaims(string username, Guid id, string role)
        {
            var claims = new List<Claim>
            {
                  new Claim(JwtRegisteredClaimNames.NameId, id.ToString()),
                  new Claim(JwtRegisteredClaimNames.Name , username),
                  new Claim(ClaimTypes.Role, role)
            };

            return claims;
        }

        private SecurityTokenDescriptor CreateTokenDescriptor(List<Claim> claims, SigningCredentials creds)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
                SigningCredentials = creds,
                Audience = _jwtSettings.Audience,
                Issuer = _jwtSettings.Issuer,
            };

            return tokenDescriptor;
        }

        private SecurityTokenDescriptor CreateRefresgTokenDescriptor(List<Claim> claims, SigningCredentials creds)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(_jwtSettings.RefreshExpiryDays),
                SigningCredentials = creds,
                Audience = _jwtSettings.Audience,
                Issuer = _jwtSettings.Issuer,
            };

            return tokenDescriptor;
        }

        public CookieOptions  GetCookieOptionsForRefreshToken()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshExpiryDays),
            };

            return cookieOptions;
        }
    }
}