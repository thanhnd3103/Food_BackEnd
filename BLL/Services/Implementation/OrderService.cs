using AutoMapper;
using BLL.Services.Interfaces;
using BLL.Utilities.LoginAccount.Interface;
using Common.Constants;
using Common.RequestObjects.Order;
using Common.ResponseObjects;
using Common.ResponseObjects.Dish;
using Common.ResponseObjects.Order;
using Common.ResponseObjects.Pagination;
using Common.Status;
using Common.Utils;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net;
using BLL.StateMachine;
using Common.Enums;
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
            // Account = _unitOfWork.AccountRepository.Get(x => x.AccountID == int.Parse(userId)).FirstOrDefault(),
            BookingPrice = totalPrice,
            Status = OrderStatus.CREATED,
            IsDeleted = false,
            BookingTime = DateTime.Now.SetKindUtc(),
            CreatedAt = DateTime.Now.SetKindUtc(),
            LastModified = DateTime.Now.SetKindUtc()
        };
        // transaction 
        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var orderCreated = _unitOfWork.OrderRepository.Insert(order);
            _unitOfWork.Save();
            var orderMapper = _unitOfWork.OrderRepository.Get(x => x.OrderID == orderCreated.OrderID,
                    includeProperties: x => x.Account)
                .FirstOrDefault();
            response = _mapper.Map<OrderResponse>(orderMapper);
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

    public ResponseObject GetOrders(GetOrdersRequest request)
    {
        // Get orders by paging
        var orders = _unitOfWork.OrderRepository.Get(
            filter: x => x.Status.Equals(OrderStatus.REPAIRED) && x.Transaction.Status.Equals(TransactionHistoryStatus.PAID),
            includeProperties: [x => x.Account, x => x.Transaction],
            orderBy: x => x.OrderBy(p => p.BookingTime),
            skipCount: (request.PageNumber - 1) * request.PageSize,
            takeCount: request.PageSize
        ).ToList();

        var ordersToGetTotalPage = _unitOfWork.OrderRepository.Get(
            filter: x => x.Status.Equals(OrderStatus.REPAIRED) && x.Transaction.Status.Equals(TransactionHistoryStatus.PAID));

        var ordersMappers = _mapper.Map<List<OrderResponse>>(orders);
        var response = new PaginationResponse()
        {
            Items = ordersMappers,
            TotalPage = (int)Math.Ceiling((decimal)ordersToGetTotalPage.Count() / request.PageSize),
            PageSize = request.PageSize,
            PageNumber = request.PageNumber,
        };
        return new ResponseObject()
        {
            Result = response,
            Message = Messages.OrderMessage.LIST_ORDER_SUCCESS,
            StatusCode = HttpStatusCode.OK
        };
    }

    public ResponseObject GetOrderDetailByOrderId(int orderId)
    {
        var order = _unitOfWork.OrderRepository.GetQueryable()
            .Where(x => x.OrderID == orderId)
            .Include(x => x.Account)
            .Include(x => x.OrderDetails)
            .ThenInclude(od => od.Dish)
            .FirstOrDefault();
        if (order is null)
            return new ResponseObject()
            {
                Result = null,
                Message = Messages.OrderMessage.GET_ORDER_FAIL,
                StatusCode = HttpStatusCode.BadRequest
            };
        var response = _mapper.Map<OrderDetailResponse>(order);
        List<OrderDishResponse> dishOrderResponse = new List<OrderDishResponse>();
        foreach (var orderDetail in order.OrderDetails)
        {
            var dishOrderMapper = _mapper.Map<OrderDishResponse>(orderDetail);
            dishOrderResponse.Add(dishOrderMapper);
        }

        response.OrderDetails = dishOrderResponse;
        return new ResponseObject()
        {
            Result = response,
            Message = Messages.OrderDetailMessage.GET_ORDER_DETAIL_BY_ID_SUCCESS,
            StatusCode = HttpStatusCode.OK
        };
    }

    public ResponseObject UpdateOrderStatus(int orderId, UpdateOrderRequest request)
    {
        var order = _unitOfWork.OrderRepository!.GetByID(orderId);
        if (order == null)
        {
            return new ResponseObject()
            {
                Result = null,
                Message = Messages.General.NO_DATA_ERROR,
                StatusCode = HttpStatusCode.NoContent,
            };
        }

        var stateMachine = new OrderStateMachine(order.Status);
        stateMachine.Fire(request.OrderEvent);
        
        //update order
        order.Status = stateMachine.CurrentState;
        order.LastModified = DateTime.Now.SetKindUtc();
        
        _unitOfWork.OrderRepository!.Update(order);
        return new ResponseObject()
        {
            Result = null,
            Message = Messages.OrderMessage.UPDATE_ORDER_SUCCESS,
            StatusCode = HttpStatusCode.OK
        };

    }

    public ResponseObject GetCurrentUserOrders(string userId)
    {
        var orders = _unitOfWork.OrderRepository.Get(
            filter: x => x.Transaction.Status.Equals(TransactionHistoryStatus.PAID) && x.Account.AccountID == Int32.Parse(userId),
            includeProperties: [x => x.Account, x => x.Transaction],
            orderBy: x => x.OrderBy(p => p.BookingTime)
        ).ToList();
        var response = _mapper.Map<List<OrderResponse>>(orders);
        return new ResponseObject()
        {
            Result = response,
            Message = Messages.OrderMessage.LIST_ORDER_SUCCESS,
            StatusCode = HttpStatusCode.OK
        };
    }
}