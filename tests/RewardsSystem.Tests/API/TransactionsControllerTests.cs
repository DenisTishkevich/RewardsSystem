using MediatR;
using Moq;
using RewardsSystem.Controllers;
using RewardsSystem.Service.Commands;
using RewardsSystem.Service.Models;

namespace RewardsSystem.Tests.API;

public class TransactionsControllerTests : ControllerTests<TransactionsController, TransactionsControllerTests.TestScope>
{
    protected override TestScope CreateTestScope()
        => new TestScope(m => new TransactionsController(m));

    [Fact]
    public async Task CustomersController_CreateCustomer_VerifyMediatr()
    {
        await VerifyMediatr<CreateTransactionCommand, int>(
            () => 1,
            (controller, scope) => controller.Create(scope.CreateTransactionRequest, AnyCancellationToken));
    }

    [Fact]
    public async Task CustomersController_CreateCustomer_CheckForCorrespondenceOfTheResponse()
    {
        await CheckForCorrespondenceOfTheResponse<CreateTransactionCommand, int>(
          () => 1,
          (controller, scope) => controller.Create(scope.CreateTransactionRequest, AnyCancellationToken));
    }

    public class TestScope : TestScopeBase<TransactionsController>
    {
        public int TransactionId { get; set; }

        public CreateTransactionView CreateTransactionRequest { get; set; }


        public TestScope(Func<IMediator, TransactionsController> factory)
            : base(factory)
        {
            TransactionId = 1;
            CreateTransactionRequest = new CreateTransactionView()
            {
                Price = 10,
                CustomerId = 1,
            };
            Mediator
                .Setup(x => x.Send(It.IsAny<CreateTransactionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TransactionId);
        }
    }
}