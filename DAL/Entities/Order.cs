using DAL.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Enums;

namespace DAL.Entities
{
#pragma warning disable CS8618
    public class Order : ISoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }
        [Required]
        public DateTime BookingTime { get; set; }
        [Required]
        public decimal BookingPrice { get; set; }
        [Required]
        [ForeignKey(nameof(Account))]
        public int AccountID { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
        [Required] 
        public DateTime CreatedAt { get; set; }
        [Required] 
        public DateTime LastModified { get; set; }
        public virtual Account Account { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
