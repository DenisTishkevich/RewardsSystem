using RewardsSystem.Domain.Entities;

namespace RewardsSystem.Tests.Mock;

public class CustomersMock
{
    public static IEnumerable<Customer> Get()
    {
        return new[]
        {
            new Customer()
            {
                Id = 1,
                Name = "Denis"

            },
            new Customer()
            {
                Id = 2,
                Name = "Artem"
            }
        };
    }
}
