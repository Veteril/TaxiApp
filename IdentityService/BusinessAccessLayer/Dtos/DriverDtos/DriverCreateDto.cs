using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Dtos
{
    public class DriverCreateDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public double Rating { get; set; } = 4.5;

        public int Experience { get; set; }
    }
}
