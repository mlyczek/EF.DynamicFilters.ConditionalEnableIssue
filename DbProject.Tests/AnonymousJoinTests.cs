using System;
using System.Data.Entity;
using System.Linq;
using EntityFramework.DynamicFilters;
using NUnit.Framework;

namespace DbProject.Tests
{
    [TestFixture]
    public class AnonymousJoinTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Database.SetInitializer(new DroppingDatabaseInitializer());
        }

        [Test]
        public void ShouldReturnAllItems()
        {
            using (var context = new TestDbContext())
            {
                context.DisableAllFilters();

                var items = context.BoughtItems.ToList();

                Assert.That(items, Has.Count.EqualTo(3));
            }
        }

        [Test]
        public void ShouldReturnResultUsingAnonymousJoin()
        {
            using (var context = new TestDbContext())
            {
                context.DisableAllFilters();

                var cartWithItems = context.ShoppingCarts
                    .Join(
                        context.BoughtItems,
                        cart => cart.Id,
                        item => item.ShoppingCartId,
                        (cart, item) => new
                        {
                            Cart = cart,
                            Item = item
                        })
                    .ToList();

                Assert.That(cartWithItems, Has.Count.EqualTo(3));
            }
        }

        [Test]
        public void ShouldNotFailWhileJoinToSelfWithFilters()
        {
            using (var context = new TestDbContext())
            {
                context.DisableAllFilters();

                var result = context.ShoppingCarts
                    .Join(
                        context.ShoppingCarts,
                        cart1 => cart1.Id,
                        cart2 => cart2.Id,
                        (cart1, cart2) => new
                        {
                            Cart1 = cart1,
                            Cart2 = cart2
                        })
                    .ToList();

                Assert.That(result, Has.Count.EqualTo(1));
            }
        }

        [Test]
        public void ShouldNotFailWhileJoinToSelfWithoutFilters()
        {
            using (var context = new TestDbContext())
            {
                var result = context.Cars
                    .Join(
                        context.Cars,
                        car1 => car1.Id,
                        car2 => car2.Id,
                        (car1, car2) => new
                        {
                            Car1 = car1,
                            Car2 = car2
                        })
                    .ToList();

                Assert.That(result, Has.Count.EqualTo(1));
            }
        }

        [Test]
        public void ShouldNotFailWithAnonymousProjection()
        {
            using (var context = new TestDbContext())
            {
                context.DisableAllFilters();

                var result = context.ShoppingCarts
                    .Select(x => new { CartName = x.Name })
                    .ToList();

                Assert.That(result, Has.Count.EqualTo(1));
            }
        }

        private class DroppingDatabaseInitializer : DropCreateDatabaseAlways<TestDbContext>
        {
            protected override void Seed(TestDbContext context)
            {
                var cart = new ShoppingCart
                {
                    Id = Guid.NewGuid(),
                    Name = "test-cart",
                };

                context.ShoppingCarts.Add(cart);

                for (var i = 0; i < 3; i++)
                {
                    var boughtItem = CreateItem(cart, i);
                    context.BoughtItems.Add(boughtItem);
                }

                var car = new Car
                {
                    Id = Guid.NewGuid(),
                    Brand = "Mazda"
                };

                context.Cars.Add(car);
            }

            private static BoughtItem CreateItem(ShoppingCart cart, int number)
            {
                return new BoughtItem
                {
                    Id = Guid.NewGuid(),
                    ShoppingCartId = cart.Id,
                    ItemName = $"item-{number}",
                    Count = number
                };
            }
        }
    }
}