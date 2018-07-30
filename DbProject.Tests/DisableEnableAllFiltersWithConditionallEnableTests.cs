using System.Data.Entity;
using System.Linq;
using EntityFramework.DynamicFilters;
using NUnit.Framework;

namespace DbProject.Tests
{
    [TestFixture]
    public class DisableEnableAllFiltersWithConditionallEnableTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Database.SetInitializer(new DroppingDatabaseInitializer());
        }

        [Test]
        public void ShouldReturnValuesBasedOnConditionallyEnabledFilter()
        {
            using (var context = new TestDbContext())
            {
                // should return filtered posts because filter is enabled
                var posts = context.BlogPosts.ToList();
                Assert.That(posts, Has.Count.EqualTo(1));

                // should return all posts because filter is disabled
                context.FilterDisabled = true;
                var filteredPosts = context.BlogPosts.ToList();
                Assert.That(filteredPosts, Has.Count.EqualTo(10));
            }
        }

        [Test]
        public void ShouldReturnValuesBasedOnConditionallyEnabledFilterAfterDisableEnableAll()
        {
            using (var context = new TestDbContext())
            {
                // disable all filters and enable them back
                context.DisableAllFilters();
                context.EnableAllFilters();

                // should return filtered posts because filter is enabled
                var posts = context.BlogPosts.ToList();
                Assert.That(posts, Has.Count.EqualTo(1));

                // should return all posts because filter is disabled
                context.FilterDisabled = true;
                var filteredPosts = context.BlogPosts.ToList();
                Assert.That(filteredPosts, Has.Count.EqualTo(10));
            }
        }

        private class DroppingDatabaseInitializer : DropCreateDatabaseAlways<TestDbContext>
        {
            protected override void Seed(TestDbContext context)
            {
                for (var i = 0; i < 10; i++)
                {
                    var post = new BlogPost
                    {
                        Number = i
                    };

                    context.BlogPosts.Add(post);
                }
            }
        }
    }
}