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
    public async Task<ActionResult<IEnumerable<CustomerView>>> Get(CancellationToken ct)
    {
        var result = await Mediator.Send(new GetCustomersQuery(), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> Create(CreateCustomerView request, CancellationToken ct)
    {
        var result = await Mediator.Send(new CreateCustomerCommand(request), ct);
        return Ok(result);
    }
}