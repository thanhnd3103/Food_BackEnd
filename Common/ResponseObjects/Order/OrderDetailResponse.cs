using Common.ResponseObjects.Account;

namespace Common.ResponseObjects.Order;

public class OrderDetailResponse
{
    public int OrderID { get; set; }
    public DateTime BookingTime { get; set; }
    public decimal BookingPrice { get; set; }
    public AccountResponse Account { get; set; }
    public List<OrderDishResponse> OrderDetails { get; set; }
}