using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
#pragma warning disable CS8618
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailID { get; set; }
        [Required]
        [ForeignKey(nameof(Dish))]
        public int DishID { get; set; }
        [Required]
        [ForeignKey(nameof(Order))]
        public int OrderID { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }

        public virtual Order Order { get; set; }
        public virtual Dish Dish { get; set; }

    }
}
