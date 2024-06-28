using AutoMapper;
using Common.RequestObjects.AuthController;
using Common.RequestObjects.Dish;
using Common.ResponseObjects.Dish;
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
            CreateMap<Dish, DishResponse>();
        }
    }
}
