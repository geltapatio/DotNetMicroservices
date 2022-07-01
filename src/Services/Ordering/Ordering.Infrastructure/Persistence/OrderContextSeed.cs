using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation($"Seed database associated with context {typeof(OrderContext).Name}");
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order() {UserName = "gel", FirstName = "Luis", LastName = "Gamiz",
                    EmailAddress = "luismiguel.gamiz@gmail.com", AddressLine = "Bahcelievler",
                    Country = "Turkey", TotalPrice = 350, CVV ="254",
                    CardName="MasterCard",
                    CardNumber="1234 1569 2547 3456" ,
                    Expiration = DateTime.Now.ToShortDateString(),
                    LastModifiedBy = "Luis Gamiz",
                    LastModifiedDate = DateTime.Now.AddDays(-5),
                    State= "BE",
                    ZipCode = "3007"
                }
            };
        }
    }
}