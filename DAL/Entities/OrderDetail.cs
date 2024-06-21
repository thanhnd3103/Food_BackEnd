using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
#pragma warning disable CS8618
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderDetailID { get; set; }
        [Required]
        [ForeignKey(nameof(Dish))]
        public Guid DishID { get; set; }
        [Required]
        [ForeignKey(nameof(Order))]
        public Guid OrderID { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }

        public virtual Order Order { get; set; }
        public virtual Dish Dish { get; set; }

    }
}
