using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Dtos
{
    public class DriverInfoDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Experience { get; set; }
    }
}
