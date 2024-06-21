using System.ComponentModel.DataAnnotations;

namespace Common.RequestObjects.AuthController
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(420)]
        [EmailAddress(ErrorMessage = "Không đúng định dạng email")]
        public required string Email { get; set; }
        [Required]
        [MaxLength(1000)]
        [RegularExpression(
            "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 8 kí tự, gồm một chữ cái in thường, một chữ cái in hoa, một kí tự số và 1 kí tự đặc biệt")]
        public required string Password { get; set; }
        [Required]
        [MaxLength(1000)]
        public required string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(200)]
        public required string FullName { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Address { get; set; }
    }
}
