using BLL.Services.Interfaces;
using Common.Enums;
using Common.RequestObjects.Dish;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public ActionResult<object> GetDishes([FromQuery] GetDishesRequest getDishesRequest)
    {
        var response = _dishService.GetDishes(getDishesRequest);
        return response;
    }

    [HttpGet("/dish/{id}")]
    [Authorize]
    public ActionResult<object> GetDish([FromRoute] int id)
    {
        return _dishService.GetDish(id);
    }

    [HttpGet("/random")]
    [Authorize]
    public ActionResult<object> RandomDish([FromQuery] Meal meal)
    {
        return _dishService.RandomDish(meal);
    }


    // [HttpPost("/dishes")]
    // public ActionResult<object> CreateDish([FromBody] CreateDishRequest createDishRequest)
    // {
    //     
    // }
}