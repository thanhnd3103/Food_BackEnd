using System.Net;
using BLL.Services.Interfaces;
using Common.Constants;
using Common.RequestObjects.Dish;
using Common.ResponseObjects;
using Common.Status;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services.Implementation;

public class DishService : IDishService
{
    private readonly IUnitOfWork _unitOfWork;

    public DishService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public ResponseObject GetDishes(GetDishesRequest getDishesRequest)
    {
        IList<Dish> response = new List<Dish>();
        var getDishes = _unitOfWork.DishRepository.Get()
            .Skip((getDishesRequest.PageNumber - 1) * getDishesRequest.PageSize)
            .Take(getDishesRequest.PageSize);
        if (getDishesRequest.Status == ModelStatus.ACTIVE)
        {
            response = searchAllActive(getDishes).ToList();
            return new ResponseObject
            {
                Result = response,
                StatusCode = HttpStatusCode.OK,
                Message = Messages.DishMessage.List_Dishes_Message_Success
            };
        }
        
        if (getDishesRequest.Status == ModelStatus.INACTIVE)
        {
            response = searchAllInActive(getDishes).ToList();
            return new ResponseObject
            {
                Result = response,
                StatusCode = HttpStatusCode.OK,
                Message = Messages.DishMessage.List_Dishes_Message_Success
            };
        }
        
        if (getDishesRequest.Name != null)
        {
            response = searchAllByName(getDishes, getDishesRequest.Name).ToList();
            return new ResponseObject
            {
                Result = response,
                StatusCode = HttpStatusCode.OK,
                Message = Messages.DishMessage.List_Dishes_Message_Success
            };
        }
        
        if (getDishesRequest.Name != null)
        {
            response = searchAllByName(getDishes, getDishesRequest.Name).ToList();
            return new ResponseObject
            {
                Result = response,
                StatusCode = HttpStatusCode.OK,
                Message = Messages.DishMessage.List_Dishes_Message_Success
            };
        }

        response = searchAllInRange(getDishes, getDishesRequest.MinPrice, getDishesRequest.MaxPrice).ToList();
        return new ResponseObject()
        {
            Result = response,
            Message = Messages.DishMessage.List_Dishes_Message_Success,
            StatusCode = HttpStatusCode.OK
        };
    }

    public ResponseObject CreateDish(CreateDishRequest createDishRequest)
    {
        throw new NotImplementedException();
    }

    private IList<Dish> searchAllActive(IQueryable<Dish> queryable)
    {
        return queryable.Where(x => x.IsDeleted == false).ToList();
    }
    
    private IList<Dish> searchAllInActive(IQueryable<Dish> queryable)
    {
        return queryable.Where(x => x.IsDeleted == true).ToList();
    }
    
    private IList<Dish> searchAllByName(IQueryable<Dish> queryable, string name)
    {
        return queryable.Where(x => x.Name.Contains(name)).ToList();
    }
    
    private IList<Dish> searchAllInRange(IQueryable<Dish> queryable, decimal? min, decimal? max)
    {
        if (min != null && max == null)
        {
            return queryable.Where(x => x.Price > min).ToList();
        }
        if (min == null && max != null)
        {
            return queryable.Where(x => x.Price < max && x.Price > 0).ToList();
        }
        if (min != null && max != null)
        {
            return queryable.Where(x => x.Price < max && x.Price > min).ToList();
        }
        return queryable.ToList();
    }
}