using AutoMapper;
using BLL.Services.Interfaces;
using Common.Constants;
using Common.Enums;
using Common.RequestObjects.Dish;
using Common.ResponseObjects;
using Common.ResponseObjects.Dish;
using Common.Status;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Net;

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
    public ResponseObject GetDish(int id)
    {
        try
        {
            var resultEntity = _unitOfWork.DishRepository.GetByID(id);
            if (resultEntity != null)
            {
                return new ResponseObject
                {
                    Result = new DishResponse
                    {
                        DishID = resultEntity.DishID,
                        Name = resultEntity.Name,
                        Price = resultEntity.Price,
                        ImageUrl = resultEntity.ImageUrl,
                    },
                    StatusCode = HttpStatusCode.OK,
                    Message = Messages.DishMessage.GET_DISH_SUCCESS,
                };
            }
            else
            {
                return new ResponseObject
                {
                    Result = null,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = Messages.General.NO_DATA_ERROR,
                };
            }
        }
        catch (Exception ex)
        {
            return new ResponseObject
            {
                Result = ex,
                StatusCode = HttpStatusCode.BadRequest,
                Message = Messages.General.CRITICAL_UNIDENTIFIED_ERROR,
            };
        }

    }
    public ResponseObject GetDishes(GetDishesRequest getDishesRequest)
    {
        IList<DishResponse> response = new List<DishResponse>();
        IList<Dish> dishes = new List<Dish>();
        var getDishes = _unitOfWork.DishRepository.Get()
            .Skip((getDishesRequest.PageNumber - 1) * getDishesRequest.PageSize)
            .Take(getDishesRequest.PageSize);
        if (getDishesRequest.Status == ModelStatus.ACTIVE)
        {
            dishes = searchAllActive(getDishes).ToList();
            response = _mapper.Map<List<DishResponse>>(dishes);
            return new ResponseObject
            {
                Result = response,
                StatusCode = HttpStatusCode.OK,
                Message = Messages.DishMessage.LIST_DISHES_MESSAGE_SUCCESS
            };
        }

        if (getDishesRequest.Status == ModelStatus.INACTIVE)
        {
            dishes = searchAllInActive(getDishes).ToList();
            response = _mapper.Map<List<DishResponse>>(dishes);
            return new ResponseObject
            {
                Result = response,
                StatusCode = HttpStatusCode.OK,
                Message = Messages.DishMessage.LIST_DISHES_MESSAGE_SUCCESS
            };
        }

        if (getDishesRequest.Name != null)
        {
            dishes = searchAllByName(getDishes, getDishesRequest.Name).ToList();
            response = _mapper.Map<List<DishResponse>>(dishes);
            return new ResponseObject
            {
                Result = response,
                StatusCode = HttpStatusCode.OK,
                Message = Messages.DishMessage.LIST_DISHES_MESSAGE_SUCCESS
            };
        }

        if (getDishesRequest.Name != null)
        {
            dishes = searchAllByName(getDishes, getDishesRequest.Name).ToList();
            return new ResponseObject
            {
                Result = response,
                StatusCode = HttpStatusCode.OK,
                Message = Messages.DishMessage.LIST_DISHES_MESSAGE_SUCCESS
            };
        }

        dishes = searchAllInRange(getDishes, getDishesRequest.MinPrice, getDishesRequest.MaxPrice).ToList();
        response = _mapper.Map<List<DishResponse>>(dishes);
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
    public ResponseObject RandomDish(Meal meal)
    {
        Tag switchTag;
        IQueryable<DishTag> dishTagList = Enumerable.Empty<DishTag>().AsQueryable();
        switch (meal)
        {
            case Meal.Breakfast:
                switchTag = _unitOfWork.TagRepository!
                    .Get(filter: tag => tag.Name.Contains("sáng") || tag.Name.Contains("Breakfast"))
                    .ToList().FirstOrDefault()!;
                dishTagList = _unitOfWork.DishTagRepository!
                    .Get(filter: dishTag => dishTag.TagID == switchTag.TagID);
                break;
            case Meal.Lunch:
                switchTag = _unitOfWork.TagRepository!
                    .Get(filter: tag => tag.Name.Contains("trưa") || tag.Name.Contains("Lunch"))
                    .ToList().FirstOrDefault()!;
                dishTagList = _unitOfWork.DishTagRepository!
                    .Get(filter: dishTag => dishTag.TagID == switchTag.TagID);
                break;
            case Meal.Dinner:
                switchTag = _unitOfWork.TagRepository!
                    .Get(filter: tag => tag.Name.Contains("tối") || tag.Name.Contains("Dinner"))
                    .ToList().FirstOrDefault()!;
                dishTagList = _unitOfWork.DishTagRepository!
                    .Get(filter: dishTag => dishTag.TagID == switchTag.TagID);
                break;
            default:
                break;
        }
        var resultList = dishTagList.ToList();
        if (resultList.IsNullOrEmpty() || resultList.Count == 0)
        {
            return new ResponseObject()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = Messages.DishMessage.NO_CONTENT,
                Result = null,
            };
        }
        else
        {
            Random random = new Random();
            int randomNumber = random.Next(0, resultList.Count);
            Dish resultEntity = _unitOfWork.DishRepository!.GetByID(resultList[randomNumber].DishID)!;
            return new ResponseObject()
            {
                StatusCode = HttpStatusCode.OK,
                Message = Messages.DishMessage.GET_DISH_SUCCESS,
                Result = new DishResponse
                {
                    DishID = resultEntity.DishID,
                    Name = resultEntity.Name,
                    Price = resultEntity.Price,
                    ImageUrl = resultEntity.ImageUrl,
                },
            };
        }
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
            return queryable.Where(x => x.Price >= min).ToList();
        }
        if (min == null && max != null)
        {
            return queryable.Where(x => x.Price <= max && x.Price > 0).ToList();
        }
        if (min != null && max != null)
        {
            return queryable.Where(x => x.Price <= max && x.Price > min).ToList();
        }
        return queryable.ToList();
    }
}