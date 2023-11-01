using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Dtos
{
    public class UserTokenDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public string Role { get; set; }
    }
}