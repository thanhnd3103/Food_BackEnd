using AutoMapper;
using BLL.Services.Interfaces;
using BLL.Utilities.AWSHelper;
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
    #region Properties
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAWSHelper _awsHelper;
    #endregion

    #region Constructer
    public DishService(IUnitOfWork unitOfWork, IMapper mapper, IAWSHelper awsHelper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _awsHelper = awsHelper;
    }
    #endregion

    #region GET METHOD
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
        if (getDishesRequest.Meal.HasValue)
        {
            Tag switchTag = new Tag();
            switch (getDishesRequest.Meal)
            {
                case Meal.Breakfast:
                    switchTag = _unitOfWork.TagRepository!
                        .Get(filter: tag => tag.Name.Contains("trưa") || tag.Name.Contains("Lunch"))
                        .ToList().FirstOrDefault()!;

                    break;
                case Meal.Lunch:
                    switchTag = _unitOfWork.TagRepository!
                        .Get(filter: tag => tag.Name.Contains("trưa") || tag.Name.Contains("Lunch"))
                        .ToList().FirstOrDefault()!;
                    break;
                case Meal.Dinner:
                    switchTag = _unitOfWork.TagRepository!
                        .Get(filter: tag => tag.Name.Contains("tối") || tag.Name.Contains("Dinner"))
                        .ToList().FirstOrDefault()!;
                    break;
                default:
                    break;
            }
            if (switchTag != null)
            {
                filter = filter == null
                    ? (dish => dish.DishTags.Any(dishTag => dishTag.TagID == switchTag.TagID))
                    : filter.And(dish => dish.DishTags.Any(dishTag => dishTag.TagID == switchTag.TagID));
            }


        }
        // Call the Get method
        dishes = _unitOfWork.DishRepository!.Get(
            filter: filter,
            skipCount: (getDishesRequest.PageNumber - 1) * getDishesRequest.PageSize,
            takeCount: getDishesRequest.PageSize,
            includeProperties: dish => dish.DishTags
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
    public ResponseObject GetDishesHomePage()
    {
        var dishes = _unitOfWork.DishRepository!.Get(orderBy: q => q.OrderByDescending(dish => dish.DishID),
                                                    takeCount: 5);
        return new ResponseObject
        {
            StatusCode = HttpStatusCode.OK,
            Message = Messages.DishMessage.LIST_DISHES_MESSAGE_SUCCESS,
            Result = _mapper.Map<List<DishResponse>>(dishes),
        };

    }
    public ResponseObject RandomDish(Meal meal)
    {
        DAL.Entities.Tag switchTag;
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
            int maxAttempts = dishTagList.Count * 2; // Maximum number of attempts to find a valid dish
            int attempt = 0;
            Dish? resultEntity = null;

            while (attempt < maxAttempts)
            {
                int randomNumber = random.Next(0, dishTagList.Count);
                resultEntity = _unitOfWork.DishRepository!.GetByID(dishTagList[randomNumber].DishID);

                if (resultEntity != null && !resultEntity.IsDeleted)
                {
                    break;
                }

                attempt++;
            }

            if (resultEntity != null && !resultEntity.IsDeleted)
            {
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
            else
            {
                return new ResponseObject()
                {
                    StatusCode = HttpStatusCode.NoContent,
                    Message = Messages.DishMessage.NO_CONTENT,
                    Result = null
                };
            }
        }
    }
    #endregion

    public ResponseObject CreateDish(CreateDishRequest createDishRequest)
    {
        var dishCheckDuplicated = _unitOfWork.DishRepository!
            .Get(filter: dish => dish.Name.Trim().ToLower().Equals(createDishRequest.Name.Trim().ToLower()))
            .FirstOrDefault();
        if (dishCheckDuplicated != null)
        {
            return new ResponseObject()
            {
                Message = Messages.DishMessage.DUPLICATED_DISH,
                Result = null,
                StatusCode = HttpStatusCode.OK,
            };
        }




        (bool isSuccess, string imageUrl) = _awsHelper.UploadImage(createDishRequest.ImageFile);

        if (isSuccess)
        {
            var dish = _mapper.Map<Dish>(createDishRequest);
            dish.ImageUrl = imageUrl;

            var response = _unitOfWork.DishRepository.Insert(dish);
            //README: Save here to get ID to insert to DishTag
            _unitOfWork.Save();

            var tagList = _unitOfWork.TagRepository!.Get();
            foreach (int tagId in createDishRequest.TagList.Distinct().ToArray())
            {
                if (tagList.Any(tag => tag.TagID == tagId))
                {
                    _unitOfWork.DishTagRepository!.Insert(new DishTag
                    {
                        TagID = tagId,
                        DishID = response.DishID,
                    });
                }
            }
            if (_unitOfWork.Save() > 0)
            {
                return new ResponseObject()
                {
                    Result = _mapper.Map<DishResponse>(response),
                    Message = Messages.DishMessage.CREATE_UPDATE_DISH_MESSAGE_SUCCESS,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseObject()
                {
                    Result = _mapper.Map<DishResponse>(response),
                    Message = Messages.DishMessage.CREATE_UPDATE_DISH_MESSAGE_SUCCESS_NO_TAG,
                    StatusCode = HttpStatusCode.OK
                };
            }

        }
        else
        {
            return new ResponseObject()
            {
                Result = imageUrl,
                Message = Messages.General.CRITICAL_UNIDENTIFIED_ERROR,
                StatusCode = HttpStatusCode.OK
            };
        }

    }

    public ResponseObject UpdateDish(UpdateDishRequest request)
    {
        #region checkNameDuplicated
        var dishCheckDuplicated = _unitOfWork.DishRepository!
            .Get(filter: dish => dish.Name.Trim().ToLower().Equals(request.Name.Trim().ToLower()))
            .FirstOrDefault();
        if (dishCheckDuplicated != null)
        {
            return new ResponseObject()
            {
                Message = Messages.DishMessage.DUPLICATED_DISH,
                Result = null,
                StatusCode = HttpStatusCode.OK,
            };
        }
        #endregion

        #region check if id existed

        var dishCheckExisted = _unitOfWork.DishRepository!
                                .Get(filter: dish => dish.DishID == request.DishId)
                                .FirstOrDefault();
        if (dishCheckExisted == null)
        {
            return new ResponseObject()
            {
                Message = Messages.DishMessage.NO_CONTENT,
                Result = null,
                StatusCode = HttpStatusCode.NoContent,
            };
        }
        #endregion
        else
        {
            #region delete old image
            (bool isDeleted, string errorMsg) = _awsHelper.DeleteImage(dishCheckExisted.ImageUrl);
            if (!isDeleted)
            {
                return new ResponseObject()
                {
                    Result = errorMsg,
                    Message = Messages.General.CRITICAL_UNIDENTIFIED_ERROR,
                    StatusCode = HttpStatusCode.OK
                };
            }
            #endregion


            (bool isSuccess, string imageUrl) = _awsHelper.UploadImage(request.ImageFile);

            if (isSuccess)
            {
                _mapper.Map(request, dishCheckExisted);
                dishCheckExisted.ImageUrl = imageUrl;
                if (!dishCheckExisted.IsDeleted)
                {
                    dishCheckExisted.DeletedAt = null;
                }

                _unitOfWork.DishRepository!.Update(dishCheckExisted);
                #region delete all current tag in DishTag table
                var currentDishTag = _unitOfWork.DishTagRepository!.Get(filter: dishTag => dishTag.DishID == request.DishId);

                foreach (var dishTag in currentDishTag)
                {
                    _unitOfWork.DishTagRepository!.Delete(dishTag);
                }
                _unitOfWork.Save();
                #endregion

                #region Add Tag
                var tagList = _unitOfWork.TagRepository!.Get();
                foreach (int tagId in request.TagList.Distinct().ToArray())
                {
                    if (tagList.Any(tag => tag.TagID == tagId))
                    {
                        _unitOfWork.DishTagRepository!.Insert(new DishTag
                        {
                            TagID = tagId,
                            DishID = dishCheckExisted.DishID,
                        });
                    }
                }
                #endregion
                if (_unitOfWork.Save() > 0)
                {
                    return new ResponseObject()
                    {
                        Result = _mapper.Map<DishResponse>(dishCheckExisted),
                        Message = Messages.DishMessage.CREATE_UPDATE_DISH_MESSAGE_SUCCESS,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    return new ResponseObject()
                    {
                        Result = _mapper.Map<DishResponse>(dishCheckExisted),
                        Message = Messages.DishMessage.CREATE_UPDATE_DISH_MESSAGE_SUCCESS_NO_TAG,
                        StatusCode = HttpStatusCode.OK
                    };
                }

            }
            else
            {
                return new ResponseObject()
                {
                    Result = imageUrl,
                    Message = Messages.General.CRITICAL_UNIDENTIFIED_ERROR,
                    StatusCode = HttpStatusCode.OK
                };
            }
        }
    }

}