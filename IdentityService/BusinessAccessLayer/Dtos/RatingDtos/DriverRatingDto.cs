using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Dtos
{
    public class DriverRatingDto
    {
        public Guid Id { get; set; }

        public int Mark { get; set; }

        public int DriverId { get; set; }
    }
}
