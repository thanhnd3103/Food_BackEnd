using Common.Enums;

namespace Common.RequestObjects.Order;

public class UpdateOrderRequest
{
    public OrderEvent OrderEvent { get; set; }
}