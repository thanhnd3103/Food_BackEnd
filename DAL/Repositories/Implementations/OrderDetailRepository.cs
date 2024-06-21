using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implementations
{
    public class OrderDetailRepository(MyDBContext context) : GenericRepository<OrderDetail>(context), IOrderDetailRepository
    {

    }
}
