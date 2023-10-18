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

        public string Phone { get; set; }

        public string Email { get; set; }

        public float Rating { get; set; }

        public DriverInfoDto DriverInfoDto { get; set; }

        public List<UserRatingDto> UserRatingDtos { get; set; }
    }
}
