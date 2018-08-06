using System;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;

namespace DbProject.Tests
{
    [TestFixture]
    public class TableSplitTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Database.SetInitializer(new DroppingDatabaseInitializer());
        }

        [Test]
        public void ShouldReadEmployeeUsingSplitTable()
        {
            using (var context = new TestDbContext())
            {
                var result = context.Employees.SingleOrDefault(x => x.Name.Contains("2"));
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Details, Is.Null);
            }
        }

        [Test]
        public void ShouldRemoveAllEmployeesUsingSplitTable()
        {
            using (var context = new TestDbContext())
            {
                context.Employees.RemoveRange(context.Employees);
                context.SaveChanges();
            }
        }

        private class DroppingDatabaseInitializer : DropCreateDatabaseAlways<TestDbContext>
        {
            protected override void Seed(TestDbContext context)
            {
                for (var i = 0; i < 10; i++)
                {
                    var id = Guid.NewGuid();
                    var employee = new Employee
                    {
                        Id = id,
                        Name = $"Employee_{i}",
                        Details = new EmployeeDetails
                        {
                            Id = id,
                            Age = 20 + i
                        }
                    };

                    context.Employees.Add(employee);
                }

                context.SaveChanges();
            }
        }
    }
}