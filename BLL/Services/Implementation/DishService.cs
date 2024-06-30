using AutoMapper;
using BLL.Services.Interfaces;
using BLL.Utilities.ExpressionExtensions;
using Common.Constants;
using Common.Enums;
using Common.RequestObjects.Dish;
using Common.ResponseObjects;
using Common.ResponseObjects.Dish;
using Common.ResponseObjects.Pagination;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
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

        // Build the filter expression
        Expression<Func<Dish, bool>>? filter = null;
        if (getDishesRequest.Status.HasValue)
        {
            filter = dish =>
                dish.IsDeleted == (getDishesRequest.Status.Value == Common.Status.ModelStatus.INACTIVE);
        }
        if (!string.IsNullOrEmpty(getDishesRequest.Name))
        {
            filter = filter == null
                ? dish => dish.Name.Contains(getDishesRequest.Name)
                : filter.And(dish => dish.Name.Contains(getDishesRequest.Name));
        }
        if (getDishesRequest.MaxPrice.HasValue)
        {
            filter = filter == null
                ? dish => dish.Price <= getDishesRequest.MaxPrice.Value
                : filter.And(dish => dish.Price <= getDishesRequest.MaxPrice.Value);
        }
        if (getDishesRequest.MinPrice.HasValue)
        {
            filter = filter == null
                ? dish => dish.Price >= getDishesRequest.MinPrice.Value
                : filter.And(dish => dish.Price >= getDishesRequest.MinPrice.Value);
        }

        // Call the Get method
        dishes = _unitOfWork.DishRepository!.Get(
            filter: filter,
            skipCount: (getDishesRequest.PageNumber - 1) * getDishesRequest.PageSize,
            takeCount: getDishesRequest.PageSize
        ).ToList();

        response = _mapper.Map<List<DishResponse>>(dishes);

        return new ResponseObject
        {
            Result = new PaginationResponse
            {
                Items = response,
                TotalPage = (int)Math.Ceiling((decimal)response.Count() / getDishesRequest.PageSize),
                PageSize = getDishesRequest.PageSize,
                PageNumber = getDishesRequest.PageNumber,
            },
            StatusCode = HttpStatusCode.OK,
            Message = Messages.DishMessage.LIST_DISHES_MESSAGE_SUCCESS
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
                Message = "Duplicated dish"
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
        List<DishTag> dishTagList = new List<DishTag>();
        switch (meal)
        {
            case Meal.Breakfast:
                switchTag = _unitOfWork.TagRepository!
                    .Get(filter: tag => tag.Name.Contains("sáng") || tag.Name.Contains("Breakfast"))
                    .FirstOrDefault()!;
                dishTagList = _unitOfWork.DishTagRepository!
                    .Get(filter: dishTag => dishTag.TagID == switchTag.TagID).ToList();
                break;
            case Meal.Lunch:
                switchTag = _unitOfWork.TagRepository!
                    .Get(filter: tag => tag.Name.Contains("trưa") || tag.Name.Contains("Lunch"))
                    .ToList().FirstOrDefault()!;
                dishTagList = _unitOfWork.DishTagRepository!
                    .Get(filter: dishTag => dishTag.TagID == switchTag.TagID).ToList();
                break;
            case Meal.Dinner:
                switchTag = _unitOfWork.TagRepository!
                    .Get(filter: tag => tag.Name.Contains("tối") || tag.Name.Contains("Dinner"))
                    .ToList().FirstOrDefault()!;
                dishTagList = _unitOfWork.DishTagRepository!
                    .Get(filter: dishTag => dishTag.TagID == switchTag.TagID).ToList();
                break;
            default:
                break;
        }
        if (dishTagList.IsNullOrEmpty() || dishTagList.Count == 0)
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
            int randomNumber = random.Next(0, dishTagList.Count);
            Dish resultEntity = _unitOfWork.DishRepository!.GetByID(dishTagList[randomNumber].DishID)!;
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

}