using RewardsSystem.Domain.Entities;


namespace RewardsSystem.Tests.Mock
{
    public class TransactionsMock
    {
        public static IEnumerable<Transaction> Get()
        {
            return new[]
            {
                new Transaction()
                {
                    Id = 1,
                    Date = DateTime.UtcNow.Date,
                    Points = 90,
                    Price = 120,
                    CustomerId = 1
                },
                new Transaction()
                {
                    Id = 2,
                    Date = DateTime.UtcNow.Date,
                    Points = 0,
                    Price = 40,
                    CustomerId = 1
                },
                new Transaction()
                {
                    Id = 3,
                    Date = DateTime.UtcNow.Date,
                    Points = 90,
                    Price = 120,
                    CustomerId = 2
                },
                new Transaction()
                {
                    Id = 4,
                    Date = DateTime.UtcNow.Date,
                    Points = 0,
                    Price = 40,
                    CustomerId = 2
                }
            };
        }
    }
}
