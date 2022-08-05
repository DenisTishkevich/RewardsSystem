using MediatR;
using Microsoft.Extensions.Logging;
using RewardsSystem.Domain.Entities;
using RewardsSystem.Domain.Exceptions;
using RewardsSystem.Persistence.DbContexts;
using RewardsSystem.Service.Models;

namespace RewardsSystem.Service.Commands;

public record CreateCustomerCommand(CreateCustomerView Customer) : IRequest<int>;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
{
    private readonly ILogger<CreateCustomerCommandHandler> _logger;
    private readonly CustomerDbContext _context;

    public CreateCustomerCommandHandler(ILogger<CreateCustomerCommandHandler> logger, CustomerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<int> Handle(CreateCustomerCommand request, CancellationToken ct)
    {
        try
        {
            Customer customer = new Customer { Name = request.Customer.Name };
            _context.Customers.Add(customer);
            _context.SaveChanges();

            _logger.LogInformation($"Customer with ID = {customer.Id} created successfully.");

            return customer.Id;
        }
        catch (Exception ex)
        {
            var errorMessage = "An error occurred while creating a customer.";
            _logger.LogError(ex, errorMessage);
            throw new DomainException(errorMessage);
        }
    }
}
