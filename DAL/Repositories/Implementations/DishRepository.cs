using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implementations
{
    public class DishRepository(MyDBContext context) : GenericRepository<Dish>(context), IDishRepository
    {
    }
}
