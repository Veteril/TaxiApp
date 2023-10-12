using BAL.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BAL.Services
{
    public class TokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value; 
        }

        public string GetToken(string username, Guid id, string role)
        {
            var creds = GetCreds();
            var claims = GetClaims(username, id, role);
            var tokenDescriptor = CreateTokenDescriptor(claims, creds);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private SigningCredentials GetCreds()
        {
            var signingCredentionals = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);

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
    }
}