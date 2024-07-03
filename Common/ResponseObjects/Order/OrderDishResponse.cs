namespace Common.ResponseObjects.Order;

public class OrderDishResponse
{
    public int OrderDetailID { get; set; }
    public String DishName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}