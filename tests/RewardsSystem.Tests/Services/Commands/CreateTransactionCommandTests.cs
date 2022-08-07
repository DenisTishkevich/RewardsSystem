using Moq;
using RewardsSystem.Domain.Exceptions;
using RewardsSystem.Service.Commands;
using RewardsSystem.Service.Models;

namespace RewardsSystem.Tests.Services.Commands
{
    public class CreateTransactionCommandTests
    {
        [Fact]
        public async void CreateTransactionCommand_CreateTransaction_CheckNotNullResponseAndTrueResult()
        {
            var scope = new TestScope();
            var result = await scope.Instance.Handle(scope.Request, It.IsAny<CancellationToken>());

            result.Should().Be(scope.ExpectedResult);
        }

        [Fact]
        public async Task CreateTransactionCommand_CreateTransaction_CheckWithNotFoundCustomerId()
        {
            var scope = new TestScope(10);

            var exception = await Record.ExceptionAsync(async () => await scope.Instance.Handle(scope.Request, It.IsAny<CancellationToken>()));
            Assert.IsType<DomainException>(exception);
            Assert.Contains("There is no customer with this ID.", exception.Message);
        }

        [Fact]
        public void CreateTransactionCommand_CreateTransaction_ControlScoringUpTo49PriceUnits()
        {
            var scope = new TestScope();

            var result = scope.Instance.CalculatePoints(49);
            result.Should().Be(0);
        }

        [Fact]
        public void CreateTransactionCommand_CreateTransaction_ControlScoringAfter50PriceUnitsButLessThan101()
        {
            var scope = new TestScope();

            var result = scope.Instance.CalculatePoints(100);
            result.Should().Be(50);
        }

        [Fact]
        public void CreateTransactionCommand_CreateTransaction_ControlScoringAfter100PriceUnits()
        {
            var scope = new TestScope();

            var result = scope.Instance.CalculatePoints(500);
            result.Should().Be(850);
        }

        public class TestScope : TestScopeBase<CreateTransactionCommand, int, CreateTransactionCommandHandler>
        {
            public CreateTransactionView RequestView { get; set; }

            public TestScope(int customerId = 1)
            {
                RequestView = new CreateTransactionView()
                {
                    Price = 100,
                    CustomerId = customerId
                };
                ExpectedResult = 5;

                Request = new CreateTransactionCommand(RequestView);
                Instance = new CreateTransactionCommandHandler(Logger, DbContext);
            }
        }
    }
}
