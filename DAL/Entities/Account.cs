using DAL.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
#pragma warning disable CS8618 // Remove unnecessary blank line
    [Index(nameof(Email), IsUnique = true)]
    public class Account : ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountID { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public bool IsAdmin { get; set; } = false;

        public virtual ICollection<Order> Orders { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
#pragma warning restore CS8618 // Remove unnecessary blank line

