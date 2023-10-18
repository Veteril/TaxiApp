using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class UserRating
    {
        public int Id { get; set; }

        public int Mark { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
