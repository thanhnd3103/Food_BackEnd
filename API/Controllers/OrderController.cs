using BLL.Services.Interfaces;
using Common.Constants;
using Common.RequestObjects.Order;
using Common.ResponseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("orders")]
    [Authorize]
    [SwaggerOperation(Summary = "Will also create a NOT PAID transaction for this order")]
    public ActionResult<ResponseObject> Order(OrderRequest request)
    {
        if (!ModelState.IsValid)
            return new ResponseObject
            {
                Message = Messages.General.MODEL_STATE_INVALID,
                StatusCode = HttpStatusCode.BadRequest,
                Result = ModelState.Values.SelectMany(v => v.Errors)
            };
        var userId = HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value;
        var response = _orderService.Order(request, userId);
        return response;
    }

    [HttpGet("orders")]
    [Authorize]
    public ActionResult<object> GetOrders([FromQuery] GetOrdersRequest request)
    {
        var response = _orderService.GetOrders(request);
        return response;
    }

    [HttpGet("orders/{orderId}")]
    [Authorize]
    public ActionResult<object> GetOrders([FromRoute] int orderId)
    {
        var response = _orderService.GetOrderDetailByOrderId(orderId);
        return response;
    }
    [HttpPut("orders/{orderId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Update an order's success status to TRUE")]
    public ActionResult<object> UpdateSuccessOrder([FromRoute] int orderId, [FromBody] UpdateOrderRequest request)
    {
        return _orderService.UpdateOrderStatus(orderId, request);
    }

    [HttpGet("orders/current")]
    [Authorize]
    [SwaggerOperation(Summary = "Get all PAID orders of current logged in user")]
    public ActionResult<object> GetCurrentUserOrders()
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value;
        return _orderService.GetCurrentUserOrders(userId);
    }


}