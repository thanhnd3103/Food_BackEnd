using Common.Status;
using Common.Utils;
using DAL.Entities;
using DAL.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Transaction = DAL.Entities.Transaction;

namespace DAL
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base()
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
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
                var connectionString = configuration.GetConnectionString("LocalDBConnection");
                optionsBuilder.UseNpgsql(connectionString);
            }
            optionsBuilder.AddInterceptors(new SoftDeleteInterceptor(), new InsertInterceptor());
        }

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


            modelBuilder
                .Entity<Transaction>()
                .Property(e => e.Status)
                .HasConversion(new EnumToStringConverter<TransactionHistoryStatus>());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.SetKindUtc(),
                            v => v.SetKindUtc()
                        ));
                    }

                    if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime?, DateTime?>(
                            v => v.SetKindUtc(),
                            v => v.SetKindUtc()
                        ));
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
