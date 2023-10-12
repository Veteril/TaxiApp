using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Client
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string Phone { get; set; }
        
        public string? Email { get; set; }

        public double Rating { get; set; } = 4.5;

        public ICollection<ClientRating> ClientRatings { get; set; }
    }
}
