using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class HashService
    {
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
