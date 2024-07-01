using BLL.Services.Interfaces;
using Common.Constants;
using Common.Enums;
using Common.RequestObjects.Dish;
using Common.ResponseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpGet("dishes")]
    [Authorize]
    public ActionResult<object> GetDishes([FromQuery] GetDishesRequest getDishesRequest)
    {
        var response = _dishService.GetDishes(getDishesRequest);
        return response;
    }

    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<object> GetDish([FromRoute] int id)
    {
        return _dishService.GetDish(id);
    }

    [HttpGet("random")]
    [Authorize]
    public ActionResult<object> RandomDish([FromQuery] Meal meal)
    {
        return _dishService.RandomDish(meal);
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public ActionResult<object> CreateDish([FromForm] CreateDishRequest createDishRequest)
    {
        if (!ModelState.IsValid)
        {
            return new ResponseObject
            {
                Message = Messages.General.MODEL_STATE_INVALID,
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Result = ModelState.Values.SelectMany(v => v.Errors)
            };
        }
        return _dishService.CreateDish(createDishRequest);
    }
}