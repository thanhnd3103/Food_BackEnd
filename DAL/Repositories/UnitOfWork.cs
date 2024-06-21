using DAL.Repositories.Implementations;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MyDBContext context;
        private IAccountRepository? accountRepository;
        private IDishRepository? dishRepository;
        private IDishTagRepository? dishTagRepository;
        private IOrderDetailRepository? orderDetailRepository;
        private IOrderRepository? orderRepository;
        private ITagRepository? tagRepository;
        private ITransactionRepository? transactionRepository;


        public UnitOfWork(MyDBContext context)
        {
            this.context = context;
        }

        public IAccountRepository AccountRepository
        {
            get
            {
                if (this.accountRepository == null)
                {
                    this.accountRepository = new AccountRepository(context);
                }
                return this.accountRepository;
            }
        }

        public IDishRepository DishRepository
        {
            get
            {
                if (this.dishRepository == null)
                {
                    this.dishRepository = new DishRepository(context);
                }
                return this.dishRepository;
            }
        }

        public IDishTagRepository DishTagRepository
        {
            get
            {
                if (this.dishTagRepository == null)
                {
                    this.dishTagRepository = new DishTagRepository(context);
                }
                return this.dishTagRepository;
            }
        }

        public IOrderDetailRepository OrderDetailRepository
        {
            get
            {
                if (this.orderDetailRepository == null)
                {
                    this.orderDetailRepository = new OrderDetailRepository(context);
                }
                return this.orderDetailRepository;
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                {
                    this.orderRepository = new OrderRepository(context);
                }
                return this.orderRepository;
            }
        }

        public ITagRepository TagRepository
        {
            get
            {
                if (this.tagRepository == null)
                {
                    this.tagRepository = new TagRepository(context);
                }
                return this.tagRepository;
            }
        }

        public ITransactionRepository TransactionRepository
        {
            get
            {
                if (this.transactionRepository == null)
                {
                    this.transactionRepository = new TransactionRepository(context);
                }
                return this.transactionRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
