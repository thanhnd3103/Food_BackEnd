using Common.Utils;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Common.RequestObjects.Dish
{
    public class UpdateDishRequest
    {
        [Required]
        public int DishId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [NotNullOrEmptyOrWhitespace(ErrorMessage = "Name cannot be empty or whitespace.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        public IFormFile ImageFile { get; set; }

        [Required, MinLength(1), Description("Tag cũ sẽ bị xóa, chỉ add các tag có trong list dưới")]
        public int[] TagList { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
