using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implementations
{
    public class AccountRepository(MyDBContext context) : GenericRepository<Account>(context), IAccountRepository
    {
    }
}
