using MediatR;
using Microsoft.Extensions.Logging;
using RewardsSystem.Domain.Entities;
using RewardsSystem.Domain.Exceptions;
using RewardsSystem.Persistence.DbContexts;
using RewardsSystem.Service.Models;

namespace RewardsSystem.Service.Commands;

public record CreateTransactionCommand(CreateTransactionView Transaction) : IRequest<int>;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, int>
{
    private readonly ILogger<CreateTransactionCommandHandler> _logger;
    private readonly CustomerDbContext _context;

    public CreateTransactionCommandHandler(ILogger<CreateTransactionCommandHandler> logger, CustomerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<int> Handle(CreateTransactionCommand request, CancellationToken ct)
    {
        if (!_context.Customers.Any(x => x.Id == request.Transaction.CustomerId))
        {
            var errorMessage = "There is no customer with this ID.";
            _logger.LogError(errorMessage);
            throw new DomainException(errorMessage);
        }

        try
        {
            Transaction transaction = new Transaction
            {
                Date = DateTime.UtcNow.Date,
                Price = request.Transaction.Price,
                Points = CalculatePoints(request.Transaction.Price),
                CustomerId = request.Transaction.CustomerId
            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            _logger.LogInformation($"Transaction with ID = {transaction.Id} created successfully.");

            return transaction.Id;
        }
        catch (Exception ex)
        {
            var errorMessage = "An error occurred while creating a transaction.";
            _logger.LogError(ex, errorMessage);
            throw new DomainException(errorMessage);
        }
    }

    public decimal CalculatePoints(decimal price)
    {
        decimal pointsAmount = 0;
        int priceInInt = (int)decimal.Truncate(price);

        if (price >= 50)
        {
            pointsAmount = priceInInt - 50;
        }
        if (price > 100)
        {
            pointsAmount += priceInInt - 100;
        }

        return pointsAmount;
    }
}
