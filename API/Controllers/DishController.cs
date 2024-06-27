using BLL.Services.Interfaces;
using Common.RequestObjects.Dish;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/api")]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpGet("/dishes")]
    public ActionResult<object> GetDishes([FromQuery] GetDishesRequest getDishesRequest)
    {
        var response = _dishService.GetDishes(getDishesRequest);
        return response;
    }

    // [HttpPost("/dishes")]
    // public ActionResult<object> CreateDish([FromBody] CreateDishRequest createDishRequest)
    // {
    //     
    // }
}