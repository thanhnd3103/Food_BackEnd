using System.ComponentModel.DataAnnotations;

namespace Common.RequestObjects.Order;

public class OrderRequest
{
    [Required(ErrorMessage = "The Dishes are required")]
    [MinLength(1)]
    [MaxLength(5)]
    public IEnumerable<OrderDishRequest> Dishes { get; set; }
}