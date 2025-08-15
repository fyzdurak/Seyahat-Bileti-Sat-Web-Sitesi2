using System.ComponentModel.DataAnnotations;

namespace BiletSatisWebApp.Models
{
    public class Passenger
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string UserType { get; set; } = "User"; // User / Admin
    }
}
