using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
#pragma warning disable CS8618
    public class DishTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DishTagID { get; set; }

        [Required]
        [ForeignKey(nameof(Dish))]
        public Guid DishID { get; set; }
        [Required]
        [ForeignKey(nameof(Tag))]
        public Guid TagID { get; set; }

        public virtual Dish Dish { get; set; }
        public virtual Tag Tag { get; set; }

    }
}
