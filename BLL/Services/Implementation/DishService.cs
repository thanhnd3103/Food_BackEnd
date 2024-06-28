using System.Net;
using AutoMapper;
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
    private readonly IMapper _mapper;

    public DishService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
                Message = Messages.DishMessage.LIST_DISHES_MESSAGE_SUCCESS
            };
        }
        
        if (getDishesRequest.Status == ModelStatus.INACTIVE)
        {
            response = searchAllInActive(getDishes).ToList();
            return new ResponseObject
            {
                Result = response,
                StatusCode = HttpStatusCode.OK,
                Message = Messages.DishMessage.LIST_DISHES_MESSAGE_SUCCESS
            };
        }
        
        if (getDishesRequest.Name != null)
        {
            response = searchAllByName(getDishes, getDishesRequest.Name).ToList();
            return new ResponseObject
            {
                Result = response,
                StatusCode = HttpStatusCode.OK,
                Message = Messages.DishMessage.LIST_DISHES_MESSAGE_SUCCESS
            };
        }
        
        if (getDishesRequest.Name != null)
        {
            response = searchAllByName(getDishes, getDishesRequest.Name).ToList();
            return new ResponseObject
            {
                Result = response,
                StatusCode = HttpStatusCode.OK,
                Message = Messages.DishMessage.LIST_DISHES_MESSAGE_SUCCESS
            };
        }

        response = searchAllInRange(getDishes, getDishesRequest.MinPrice, getDishesRequest.MaxPrice).ToList();
        return new ResponseObject()
        {
            Result = response,
            Message = Messages.DishMessage.LIST_DISHES_MESSAGE_SUCCESS,
            StatusCode = HttpStatusCode.OK
        };
    }

    public ResponseObject CreateDish(CreateDishRequest createDishRequest)
    {
        var dishCheckDuplicated = _unitOfWork.DishRepository
            .Get(filter: dish => dish.Name.ToLower().Equals(createDishRequest.Name.ToLower()))
            .FirstOrDefault();
        if (dishCheckDuplicated != null)
            return new ResponseObject()
            {

            };
        var dish = _mapper.Map<Dish>(createDishRequest);
        var response = _unitOfWork.DishRepository.Insert(dish);
        _unitOfWork.Save();
        return new ResponseObject()
        {
            Result = response,
            Message = Messages.DishMessage.CREATE_DISH_MESSAGE_SUCCESS,
            StatusCode = HttpStatusCode.OK
        };
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