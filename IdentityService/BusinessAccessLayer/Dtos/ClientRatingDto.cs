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
        public int Id { get; set; }

        public int Mark { get; set; }

        public int? ClientId { get; set; }
    }
}
