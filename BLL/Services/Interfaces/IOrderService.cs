using Common.RequestObjects.Order;
using Common.ResponseObjects;

namespace BLL.Services.Interfaces;

public interface IOrderService
{
    ResponseObject Order(OrderRequest request, string userId);
    ResponseObject GetOrders(GetOrdersRequest request);
    ResponseObject GetOrderDetailByOrderId(int orderId);
    ResponseObject UpdateOrderStatus(int orderId, UpdateOrderRequest request);
    ResponseObject GetCurrentUserOrders(string userId);
}