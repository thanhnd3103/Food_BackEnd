using System.ComponentModel.DataAnnotations;

namespace Common.RequestObjects.AuthController
{
    public class LoginRequest
    {
        [Required]
        [MaxLength(420)]
        public required string Email { get; set; }
        [Required]
        [MaxLength(1000)]
        public required string Password { get; set; }
    }
}
