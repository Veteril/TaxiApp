using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public  class Driver
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public double Rating { get; set; } = 4.5;

        public int Experience { get; set; }

        public ICollection<DriverRating> DriverRatings { get; set;}
    }
}
