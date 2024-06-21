using DAL.Entities;
using DAL.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class MyDBContext : DbContext
    {
        public MyDBContext()
        {

        }
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishTag> DishTags { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasOne(i => i.Order)
                .WithMany(i => i.OrderDetails)
                .HasForeignKey(i => i.OrderID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(i => i.Dish)
                .WithMany(i => i.OrderDetails)
                .HasForeignKey(i => i.DishID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DishTag>()
                .HasOne(i => i.Tag)
                .WithMany(i => i.DishTags)
                .HasForeignKey(i => i.TagID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DishTag>()
                .HasOne(i => i.Dish)
                .WithMany(i => i.DishTags)
                .HasForeignKey(i => i.DishID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
