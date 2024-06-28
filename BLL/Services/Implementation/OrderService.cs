// using System.Net;
// using AutoMapper;
// using BLL.Services.Interfaces;
// using BLL.Utilities.LoginAccount;
// using BLL.Utilities.LoginAccount.Interface;
// using Common.Constants;
// using Common.RequestObjects.Order;
// using Common.ResponseObjects;
// using Common.ResponseObjects.Dish;
// using DAL.Entities;
// using DAL.Repositories;
//
// namespace BLL.Services.Implementation;
//
// public class OrderService : IOrderService
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly ICurrentLoginAccount _currentLoginAccount;
//     private readonly IMapper _mapper;
//
//     public OrderService(IUnitOfWork unitOfWork, ICurrentLoginAccount currentLoginAccount, 
//         IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _currentLoginAccount = currentLoginAccount;
//         _mapper = mapper;
//     }
//
//     public ResponseObject Order(OrderRequest request)
//     {
//         // local variable
//         decimal totalPrice = 0;
//         
//         string loggedInAccountId = _currentLoginAccount.getAccount();
//         if (loggedInAccountId is null)
//         {
//             return new ResponseObject()
//             {
//                 Message = Messages.AuthController.LOGIN_INTERNAL_ERROR,
//                 Result = null,
//                 StatusCode = HttpStatusCode.InternalServerError
//             };
//         }
//
//         IList<DishResponse> dishes = new List<DishResponse>();
//         foreach (var dish in request.Dishes)
//         {
//             var dishCheck = _unitOfWork.DishRepository.
//                 Get(x => x.DishID == dish.DishId).FirstOrDefault();
//             if (dishCheck is null)
//                 return new ResponseObject()
//                 {
//                     Result = null,
//                     Message = Messages.OrderMessage.PASS_WRONG_ID,
//                     StatusCode = HttpStatusCode.BadRequest
//                 };
//             var dishResponse = _mapper.Map<DishResponse>(dishCheck);
//             dishes.Add(dishResponse);
//             totalPrice += dishCheck.Price * dish.Quantity;
//         }
//         var order = new Order()
//         {
//             AccountID = int.Parse(loggedInAccountId),
//             BookingPrice = totalPrice,
//             IsDeleted = false,
//             BookingTime = DateTime.Now
//         };
//         
//     }
// }