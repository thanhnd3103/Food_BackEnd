using AutoMapper;
using BLL.Services.Interfaces;
using BLL.Utilities.LoginAccount.Interface;
using Common.Constants;
using Common.RequestObjects.Order;
using Common.ResponseObjects;
using Common.ResponseObjects.Dish;
using Common.ResponseObjects.Order;
using Common.Status;
using Common.Utils;
using DAL.Entities;
using DAL.Repositories;
using System.Net;
using Transaction = DAL.Entities.Transaction;

namespace BLL.Services.Implementation;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentLoginAccount _currentLoginAccount;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, ICurrentLoginAccount currentLoginAccount,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentLoginAccount = currentLoginAccount;
        _mapper = mapper;
    }

    public ResponseObject Order(OrderRequest request, string userId)
    {
        // local variable
        decimal totalPrice = 0;
        //response
        OrderResponse response = null;

        // string userId = _currentLoginAccount.getAccount();
        if (userId is null)
        {
            return new ResponseObject()
            {
                Message = Messages.AuthController.LOGIN_INTERNAL_ERROR,
                Result = null,
                StatusCode = HttpStatusCode.InternalServerError
            };
        }

        IList<DishResponse> dishes = new List<DishResponse>();
        foreach (var dish in request.Dishes)
        {
            var dishCheck = _unitOfWork.DishRepository.
                Get(x => x.DishID == dish.DishId).FirstOrDefault();
            if (dishCheck is null || dishCheck.IsDeleted == true)
                return new ResponseObject()
                {
                    Result = null,
                    Message = Messages.OrderMessage.PASS_WRONG_ID,
                    StatusCode = HttpStatusCode.BadRequest
                };
            var dishResponse = _mapper.Map<DishResponse>(dishCheck);
            dishes.Add(dishResponse);
            totalPrice += dishCheck.Price * dish.Quantity;
        }
        var order = new Order()
        {
            AccountID = int.Parse(userId),
            BookingPrice = totalPrice,
            IsDeleted = false,
            BookingTime = DateTime.Now.SetKindUtc()
        };
        // transaction 
        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var orderCreated = _unitOfWork.OrderRepository.Insert(order);
            response = _mapper.Map<OrderResponse>(orderCreated);
            _unitOfWork.Save();
            foreach (var dish in request.Dishes)
            {
                var dishCheck = _unitOfWork.DishRepository.
                    Get(x => x.DishID == dish.DishId).FirstOrDefault();
                if (dishCheck is null)
                    return new ResponseObject()
                    {
                        Result = null,
                        Message = Messages.OrderMessage.PASS_WRONG_ID,
                        StatusCode = HttpStatusCode.BadRequest
                    };
                var orderDetail = new OrderDetail()
                {
                    DishID = dishCheck.DishID,
                    Price = dishCheck.Price,
                    Quantity = dish.Quantity,
                    OrderID = orderCreated.OrderID
                };
                _ = _unitOfWork.OrderDetailRepository.Insert(orderDetail);
                _unitOfWork.Save();
            }

            var transactionHistory = new Transaction()
            {
                OrderID = orderCreated.OrderID,
                Status = TransactionHistoryStatus.NOTPAID,
                IsDeleted = false,
                TotalPrice = totalPrice,
                TransactionDate = DateTime.Now
            };
            _ = _unitOfWork.TransactionRepository.Insert(transactionHistory);
            _unitOfWork.Save();

            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            return new ResponseObject()
            {
                Message = Messages.General.CRITICAL_UNIDENTIFIED_ERROR,
                StatusCode = HttpStatusCode.InternalServerError,
                Result = null
            };
        }

        return new ResponseObject()
        {
            Message = Messages.OrderMessage.ORDER_SUCCESS,
            StatusCode = HttpStatusCode.OK,
            Result = response
        };
    }
}