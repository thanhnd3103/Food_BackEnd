using DAL.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
#pragma warning disable CS8618
    public class Order : ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderID { get; set; }
        [Required]
        public DateTime BookingTime { get; set; }
        [Required]
        public decimal BookingPrice { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        [ForeignKey(nameof(Account))]
        public Guid AccountID { get; set; }
        [ForeignKey(nameof(Transaction))]
        public Guid TransactionID { get; set; }
        public virtual Account Account { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
