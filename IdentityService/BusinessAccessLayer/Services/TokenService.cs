using BAL.Authentication;
using BAL.Dtos;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class TokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value; 
        }

        public string GetToken(string username, Guid id)
        {
            var creds = GetCreds();
            var claims = GetClaims(username, id);
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

        private List<Claim> GetClaims(string username, Guid id)
        {
            var claims = new List<Claim>
            {
                  new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                  new Claim(ClaimTypes.Name, username),
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

        public List<string> CreateHash(string password)
        {
            using var hmac = new HMACSHA256();
           
            var hash = new List<string>
            {
              new string(Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)))),
              new string(Convert.ToBase64String(hmac.Key))
            };

            return hash;
        }

        public string ComputeHash(string password, string salt)
        {
            using var hmac = new HMACSHA256(Convert.FromBase64String(salt));
            var computeHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

            return computeHash;
        }
    }
}
