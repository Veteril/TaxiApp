using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Dtos
{
    public class UserRatingDto
    {
        public Guid Id { get; set; }

        public int Mark { get; set; }

        public Guid UserId { get; set; }
    }
}