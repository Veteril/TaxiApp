using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Dtos
{
    public class ClientCreateDto
    {
        public string Username { get; set; }

        public string Password { get; set; } 

        public string Phone { get; set; }

        public string? Email { get; set; }
    }
}
