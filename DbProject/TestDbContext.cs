using System;
using System.Data.Entity;
using EntityFramework.DynamicFilters;

namespace DbProject
{
    public class TestDbContext : DbContext
    {
        public TestDbContext() : base("EntityConnectionString")
        {
            Configuration.LazyLoadingEnabled = false;

            Database.Log = Console.WriteLine;
        }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeDetails> EmployeeDetails { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<BoughtItem> BoughtItems { get; set; }

        public DbSet<Car> Cars { get; set; }

        public bool FilterDisabled { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BlogPost>().HasKey(p => p.Id);

            modelBuilder.Entity<Employee>()
                .HasKey(e => e.Id)
                .ToTable("Employee")
                .HasRequired(e => e.Details)
                .WithRequiredPrincipal();

            modelBuilder.Entity<EmployeeDetails>()
                .HasKey(e => e.Id)
                .ToTable("Employee");

            modelBuilder.Entity<ShoppingCart>().HasKey(e => e.Id);
            modelBuilder.Entity<BoughtItem>().HasKey(e => e.Id);

            modelBuilder.Entity<Car>().HasKey(e => e.Id);

            modelBuilder.Filter("Carts", (ShoppingCart c) => c.Name, "test");
            modelBuilder.Filter("Items", (BoughtItem i) => i.ItemName, "test");

            const string FilterName = "NumberFilter";
            modelBuilder.Filter(FilterName, (BlogPost p, int number) => p.Number == number, 5);
            modelBuilder.EnableFilter(FilterName, (TestDbContext ctx) => !ctx.FilterDisabled);
        }
    }
}