using System.ComponentModel.DataAnnotations;

namespace Common.RequestObjects.Account
{
    public class UpdateAccountRequest
    {
        [Required]
        [MaxLength(500)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; }
    }
}
