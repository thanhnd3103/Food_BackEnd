using Common.Enums;
using Common.RequestObjects.Dish;
using Common.ResponseObjects;

namespace BLL.Services.Interfaces;

public interface IDishService
{
    public ResponseObject GetDish(int id);
    public ResponseObject GetDishes(GetDishesRequest getDishesRequest);
    public ResponseObject CreateDish(CreateDishRequest createDishRequest);
    public ResponseObject RandomDish(Meal meal);
    public ResponseObject UpdateDish(UpdateDishRequest request);
}