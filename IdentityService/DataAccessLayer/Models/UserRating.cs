using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class UserRating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Mark { get; set; }

        public int? ClientId { get; set; }
        public Client Client { get; set; }

        public int? DriverId { get; set; }
        public Client Driver{ get; set; }
    }
}
