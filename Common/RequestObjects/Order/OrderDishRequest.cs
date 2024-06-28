using System.ComponentModel.DataAnnotations;

namespace Common.RequestObjects.Order;

public class OrderDishRequest
{
    [Required(ErrorMessage = "The Dish Id is required")]
    public int DishId { get; set; }
    
    [Required(ErrorMessage = "The Quantity is required")]
    [Range(minimum: 1, maximum: 10, ErrorMessage = "The quantity must be in 0 and 10")]
    public int Quantity { get; set; }
}