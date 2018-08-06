using System.Data.Entity;
using EntityFramework.DynamicFilters;

namespace DbProject
{
    public class TestDbContext : DbContext
    {
        public TestDbContext() : base("EntityConnectionString")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeDetails> EmployeeDetails { get; set; }

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

            const string FilterName = "NumberFilter";
            modelBuilder.Filter(FilterName, (BlogPost p, int number) => p.Number == number, 5);
            modelBuilder.EnableFilter(FilterName, (TestDbContext ctx) => !ctx.FilterDisabled);
        }
    }
}