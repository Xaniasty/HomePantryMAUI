using System.ComponentModel.DataAnnotations;

namespace HomePantry.Models
{
    public class LoginRequest
    {
        [Required]
        public string EmailOrLogin { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}