using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implementations
{
    public class DishTagRepository(MyDBContext context) : GenericRepository<DishTag>(context), IDishTagRepository
    {
    }
}
