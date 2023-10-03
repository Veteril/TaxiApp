using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Dtos
{
    public class DriverReadDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public double Rating { get; set; }

        public int Experience { get; set; }

        public List<DriverRatingDto> driverRatingDtos { get; set; }
    }
}
