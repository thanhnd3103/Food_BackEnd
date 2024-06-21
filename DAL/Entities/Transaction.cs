using DAL.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
#pragma warning disable CS8618
    public class Transaction : ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionID { get; set; }

        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }

        public virtual Order Order { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
