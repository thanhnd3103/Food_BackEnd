using Common.RequestObjects.Order;
using Common.ResponseObjects;

namespace BLL.Services.Interfaces;

public interface IOrderService
{
    ResponseObject Order(OrderRequest request);
}