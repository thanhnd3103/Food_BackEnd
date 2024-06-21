using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implementations
{
    public class TransactionRepository(MyDBContext context) : GenericRepository<Transaction>(context), ITransactionRepository
    {
    }
}
