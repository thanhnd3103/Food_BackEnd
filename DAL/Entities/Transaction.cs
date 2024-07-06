using Common.Status;
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
        public int TransactionID { get; set; }

        public string? BankCode { get; set; }
        
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public TransactionHistoryStatus Status { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderID { get; set; }

        public virtual Order Order { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
