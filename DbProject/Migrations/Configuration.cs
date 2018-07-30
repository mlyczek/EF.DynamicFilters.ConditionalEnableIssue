using System.Data.Entity.Migrations;

namespace DbProject.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DbProject.TestDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}