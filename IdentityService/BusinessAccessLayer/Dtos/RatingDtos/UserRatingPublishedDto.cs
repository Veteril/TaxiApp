using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Dtos.RatingDtos
{
    public class UserRatingPublishedDto
    {
        public string UserId { get; set; }

        public int Mark { get; set; }

        public string Event {  get; set; }
    }
}
