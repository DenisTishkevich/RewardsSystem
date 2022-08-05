using MediatR;
using Microsoft.AspNetCore.Mvc;
using RewardsSystem.Service.Commands;
using RewardsSystem.Service.Models;

namespace RewardsSystem.Controllers;

[ApiController]
[Route("[controller]")]
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
        return await Mediator.Send(new CreateTransactionCommand(request), ct);
    }
}