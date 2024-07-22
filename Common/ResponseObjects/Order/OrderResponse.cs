using Common.ResponseObjects.Account;

namespace Common.ResponseObjects.Order;

public class OrderResponse
{
    public int OrderID { get; set; }
    public DateTime BookingTime { get; set; }
    public decimal BookingPrice { get; set; }
    public DateTime LastModified { get; set; }
    public AccountResponse Account { get; set; }
}