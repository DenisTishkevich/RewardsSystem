using MediatR;
using Moq;
using RewardsSystem.Controllers;
using RewardsSystem.Service.Commands;
using RewardsSystem.Service.Models;
using RewardsSystem.Service.Queries;

namespace RewardsSystem.Tests.API;

public class CustomersControllerTests: ControllerTests<CustomersController, CustomersControllerTests.TestScope>
{
    protected override TestScope CreateTestScope()
        => new TestScope(m => new CustomersController(m));

    [Fact]
    public async Task CustomersController_GetCustomers_VerifyMediatr()
    {
        await VerifyMediatr<GetCustomersQuery, IEnumerable<CustomerView>>(
            Enumerable.Empty<CustomerView>,
            (controller, scope) => controller.Get(AnyCancellationToken));
    }

    [Fact]
    public async Task CustomersController_GetCustomers_CheckForCorrespondenceOfTheResponse()
    {
        await CheckForCorrespondenceOfTheResponse<GetCustomersQuery, IEnumerable<CustomerView>>(
           Enumerable.Empty<CustomerView>,
           (controller, scope) => controller.Get(AnyCancellationToken));
    }

    [Fact]
    public async Task CustomersController_CreateCustomer_VerifyMediatr()
    {
        await VerifyMediatr<CreateCustomerCommand, int>(
            () => 1,
            (controller, scope) => controller.Create(scope.CreateCustomerRequest, AnyCancellationToken));
    }

    [Fact]
    public async Task CustomersController_CreateCustomer_CheckForCorrespondenceOfTheResponse()
    {
        await CheckForCorrespondenceOfTheResponse<CreateCustomerCommand, int>(
          () => 1,
          (controller, scope) => controller.Create(scope.CreateCustomerRequest, AnyCancellationToken));
    }

    public class TestScope : TestScopeBase<CustomersController>
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public CreateCustomerView CreateCustomerRequest { get; set; }


        public TestScope(Func<IMediator, CustomersController> factory)
            : base(factory)
        {
        
            CustomerId = 1;
            CustomerName = "Denis";
            CreateCustomerRequest = new CreateCustomerView()
            {
                Name = CustomerName,
            };
            Mediator
                .Setup(x => x.Send(It.IsAny<GetCustomersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<CustomerView>());
            Mediator
                .Setup(x => x.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(CustomerId);
        }
    }
}