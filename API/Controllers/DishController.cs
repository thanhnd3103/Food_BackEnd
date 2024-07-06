using BLL.Services.Interfaces;
using Common.Constants;
using Common.Enums;
using Common.RequestObjects.Dish;
using Common.ResponseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(Summary = "Return dishes with request's conditions and paging (Status: 0 - inactive, 1 - active)")]
    public ActionResult<object> GetDishes([FromQuery] GetDishesRequest getDishesRequest)
    {
        var response = _dishService.GetDishes(getDishesRequest);
        return response;
    }
    [HttpGet("home")]
    [Authorize]
    [SwaggerOperation(Summary = "Return first 5 latest dishes")]
    public ActionResult<object> GetDishesHomePage()
    {
        var response = _dishService.GetDishesHomePage();
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
    [SwaggerOperation(Summary = "Will return 1 random dish (0 - breakfast, 1 - lunch, 2 - dinner) or none at all")]
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
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public ActionResult<object> UpdateDish([FromForm] UpdateDishRequest updateDishRequest)
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
        return _dishService.UpdateDish(updateDishRequest);
    }
}