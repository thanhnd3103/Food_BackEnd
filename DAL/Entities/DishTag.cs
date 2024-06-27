using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
#pragma warning disable CS8618
    public class DishTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DishTagID { get; set; }

        [Required]
        [ForeignKey(nameof(Dish))]
        public int DishID { get; set; }
        [Required]
        [ForeignKey(nameof(Tag))]
        public int TagID { get; set; }

        public virtual Dish Dish { get; set; }
        public virtual Tag Tag { get; set; }

    }
}
