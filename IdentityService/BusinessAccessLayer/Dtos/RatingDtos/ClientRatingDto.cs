using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Dtos
{
    public class ClientRatingDto
    {
        public Guid Id { get; set; }

        public int Mark { get; set; }

        public Guid ClientId { get; set; }
    }
}
