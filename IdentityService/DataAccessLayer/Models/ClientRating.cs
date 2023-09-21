using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class ClientRating
    {
        public int Id { get; set; }

        public int Mark { get; set; }

        public Guid ClientId { get; set; }
        public Client Client { get; set; }
    }
}