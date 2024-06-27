using System.ComponentModel.DataAnnotations;

namespace Common.RequestObjects.Dish;

public class CreateDishRequest
{
    [Required(ErrorMessage = "The Name is required")] 
    public string Name { get; set; }
    
    [Required(ErrorMessage = "The Price is required")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "The Image is required")]
    public string Image { get; set; }
}