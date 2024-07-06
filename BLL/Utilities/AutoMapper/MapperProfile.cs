using AutoMapper;
using Common.RequestObjects.AuthController;
using Common.RequestObjects.Dish;
using Common.ResponseObjects.Account;
using Common.ResponseObjects.Dish;
using Common.ResponseObjects.Order;
using Common.ResponseObjects.Tag;
using Common.ResponseObjects.Transaction;
using DAL.Entities;

namespace BLL.Utilities.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterRequest, Account>()
                .BeforeMap((_, dest) => dest.IsAdmin = false)
                .ForMember(dest => dest.PasswordHash,
                            src => src.MapFrom(x => BCrypt.Net.BCrypt.HashPassword(x.Password)));

            CreateMap<Account, RegisterRequest>();
            CreateMap<CreateDishRequest, Dish>();
            CreateMap<UpdateDishRequest, Dish>();
            CreateMap<Dish, DishResponse>();
            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account));
            CreateMap<Account, AccountResponse>();
            CreateMap<Tag, TagResponse>();

            CreateMap<OrderDetail, OrderDishResponse>()
                .ForMember(dest => dest.DishName, opt => opt.MapFrom(src => src.Dish.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Dish.ImageUrl));

            CreateMap<Order, OrderDetailResponse>()
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account));
            CreateMap<Transaction, TransactionResponse>();
        }
    }
}
