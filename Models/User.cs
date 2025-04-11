using System.ComponentModel.DataAnnotations;

namespace MedicineStoreAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; } // For real apps, store hashed passwords!
    }
}
