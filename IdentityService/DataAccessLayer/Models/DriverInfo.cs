using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class DriverInfo
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Experience { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
