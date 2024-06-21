using DAL.Repositories.Interfaces;

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
        void Save();
        void Dispose();
    }

}
