using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implementations
{
    public class OrderRepository(MyDBContext context) : GenericRepository<Order>(context), IOrderRepository
    {
    }
}
