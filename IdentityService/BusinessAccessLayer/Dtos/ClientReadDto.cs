using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Dtos
{
    public class ClientReadDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Phone { get; set; }

        public string? Email { get; set; }

        public float Rating { get; set; }

        public IEnumerable<ClientRatingDto> clientRatings { get; set; }
    }
}
