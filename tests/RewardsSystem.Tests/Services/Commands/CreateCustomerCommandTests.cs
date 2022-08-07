using Moq;
using RewardsSystem.Domain.Exceptions;
using RewardsSystem.Service.Commands;
using RewardsSystem.Service.Models;

namespace RewardsSystem.Tests.Services.Commands
{
    public class CreateCustomerCommandTests
    {
        [Fact]
        public async void CreateCustomerCommand_CreateCustomer_CheckNotNullResponseAndTrueResult()
        {
            var scope = new TestScope();
            var result = await scope.Instance.Handle(scope.Request, It.IsAny<CancellationToken>());

            result.Should().Be(scope.ExpectedResult);
        }

        [Fact]
        public async void CreateCustomerCommand_CreateCustomer_CheckProblemWithDatabase()
        {
            var scope = new TestScope();
            scope.DbContext.Database.EnsureDeleted();

            var exception = await Record.ExceptionAsync(async () => await scope.Instance.Handle(scope.Request, It.IsAny<CancellationToken>()));
            Assert.IsType<DomainException>(exception);
            Assert.Contains("An error occurred while creating a customer.", exception.Message);

        }

        public class TestScope : TestScopeBase<CreateCustomerCommand, int, CreateCustomerCommandHandler>
        {
            public CreateCustomerView RequestView { get; set; }

            public TestScope()
            {
                RequestView = new CreateCustomerView()
                {
                    Name = "Denis"
                };
                ExpectedResult = 3;

                Request = new CreateCustomerCommand(RequestView);
                Instance = new CreateCustomerCommandHandler(Logger, DbContext);
            }
        }
    }
}
