using Moq;
using RewardsSystem.Service.Models;
using RewardsSystem.Service.Queries;

namespace RewardsSystem.Tests.Services.Queries;

public class GetCustomersQueryTests
{
    [Fact]
    public async void GetCustomersQuery_GetCustomers_CheckNotNullResponseAndTrueResult()
    {
        var scope = new TestScope();
        var result = await scope.Instance.Handle(scope.Request, It.IsAny<CancellationToken>());

        result.Should().BeEquivalentTo(scope.ExpectedResult);
    }

    public class TestScope : TestScopeBase<GetCustomersQuery, IEnumerable<CustomerView>, GetCustomersQueryHandler>
    {
        public TestScope()
        {
            ExpectedResult = new[]
            {
                new CustomerView()
                {
                    Id = 1,
                    Name = "Denis",
                    PointsAmount = 90,
                    PointsAmountPerMonth = new[]
                    {
                        new TransactionPerMonthView()
                        {
                            Year = DateTime.UtcNow.Date.Year,
                            Month = DateTime.UtcNow.Date.Month,
                            Points = 90
                        }
                    },
                    Transactions = new[]
                    {
                        new TransactionView()
                        {
                            Date = DateTime.Now.Date,
                            Price = 120,
                            Points = 90
                        },
                        new TransactionView()
                        {
                            Date = DateTime.Now.Date,
                            Price = 40,
                            Points = 0
                        }
                    }
                },
                new CustomerView()
                {
                    Id = 2,
                    Name = "Artem",
                    PointsAmount = 90,
                    PointsAmountPerMonth = new[]
                    {
                        new TransactionPerMonthView()
                        {
                            Year = DateTime.UtcNow.Date.Year,
                            Month = DateTime.UtcNow.Date.Month,
                            Points = 90
                        }
                    },
                    Transactions = new[]
                    {
                        new TransactionView()
                        {
                            Date = DateTime.Now.Date,
                            Price = 120,
                            Points = 90
                        },
                        new TransactionView()
                        {
                            Date = DateTime.Now.Date,
                            Price = 40,
                            Points = 0
                        }
                    }
                }
            };

            Request = new GetCustomersQuery();
            Instance = new GetCustomersQueryHandler(Logger, Mapper, DbContext);
        }
    }
}
