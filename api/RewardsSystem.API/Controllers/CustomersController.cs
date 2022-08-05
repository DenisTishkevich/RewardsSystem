using MediatR;
using Microsoft.AspNetCore.Mvc;
using RewardsSystem.Service.Commands;
using RewardsSystem.Service.Models;
using RewardsSystem.Service.Queries;

namespace RewardsSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    public IMediator Mediator { get; }

    public CustomersController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<CustomerView>> Get(CancellationToken ct)
    {
        return await Mediator.Send(new GetCustomersQuery(), ct);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> Create(CreateCustomerView request, CancellationToken ct)
    {
        return await Mediator.Send(new CreateCustomerCommand(request), ct);
    }
}