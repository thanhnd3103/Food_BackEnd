using System.Net;
using BLL.Services.Interfaces;
using Common.Constants;
using Common.RequestObjects.Order;
using Common.ResponseObjects;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/api")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("/order")]
    public ActionResult<ResponseObject> Order(OrderRequest request)
    {
        if (!ModelState.IsValid)
            return new ResponseObject
            {
                Message = Messages.General.MODEL_STATE_INVALID,
                StatusCode = HttpStatusCode.BadRequest,
                Result = ModelState.Values.SelectMany(v => v.Errors)
            };
        var response = _orderService.Order(request);
        return response;
    }
}