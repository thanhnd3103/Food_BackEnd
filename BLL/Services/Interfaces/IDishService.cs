using Common.RequestObjects.Dish;
using Common.ResponseObjects;

namespace BLL.Services.Interfaces;

public interface IDishService
{
    public ResponseObject GetDishes(GetDishesRequest getDishesRequest);
    public ResponseObject CreateDish(CreateDishRequest createDishRequest);
}