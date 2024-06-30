using DAL.Repositories.Interfaces;
using System.Data;

namespace DAL.Repositories
{
    public interface IUnitOfWork
    {
        IAccountRepository? AccountRepository { get; }
        IDishRepository? DishRepository { get; }
        IDishTagRepository? DishTagRepository { get; }
        IOrderDetailRepository? OrderDetailRepository { get; }
        IOrderRepository? OrderRepository { get; }
        ITagRepository? TagRepository { get; }
        ITransactionRepository? TransactionRepository { get; }
        int Save();
        void Dispose();
        IDbTransaction BeginTransaction();
    }

}
