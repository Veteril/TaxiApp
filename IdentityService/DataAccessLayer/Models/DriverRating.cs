using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class DriverRating
    {
        public int Id { get; set; }

        public int Mark { get; set; }

        public int DriverId { get; set; }
        public Driver Driver { get; set; }
    }
}