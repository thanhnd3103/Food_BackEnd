using BLL.Services.Interfaces;
using Common.Constants;
using Common.RequestObjects.Order;
using Common.ResponseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    [Authorize]
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
}