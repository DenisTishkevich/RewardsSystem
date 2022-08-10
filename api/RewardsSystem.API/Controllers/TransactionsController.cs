using MediatR;
using Microsoft.AspNetCore.Mvc;
using RewardsSystem.Service.Commands;
using RewardsSystem.Service.Models;

namespace RewardsSystem.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionsController : ControllerBase
{
    public IMediator Mediator { get; }

    public TransactionsController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> Create(CreateTransactionView request, CancellationToken ct)
    {
        var result = await Mediator.Send(new CreateTransactionCommand(request), ct);
        return Ok(result);
    }
}