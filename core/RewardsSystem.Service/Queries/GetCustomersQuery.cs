using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RewardsSystem.Domain.Exceptions;
using RewardsSystem.Persistence.DbContexts;
using RewardsSystem.Service.Models;

namespace RewardsSystem.Service.Queries
{
    public record GetCustomersQuery() : IRequest<IEnumerable<CustomerView>>;

    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerView>>
    {
        private readonly ILogger<GetCustomersQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly CustomerDbContext _context;

        public GetCustomersQueryHandler(ILogger<GetCustomersQueryHandler> logger, IMapper mapper, CustomerDbContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<CustomerView>> Handle(GetCustomersQuery request, CancellationToken ct)
        {
            try
            {
                var customers = _context.Customers
                    .GroupJoin(_context.Transactions,
                        u => u.Id,
                        c => c.CustomerId,
                        (u, c) => new
                        {
                            Id = u.Id,
                            Name = u.Name,
                            Transactions = c
                        })
                    .SelectMany(
                        x => x.Transactions.DefaultIfEmpty(),
                       (customer, transaction) => new
                       {
                           Id = customer.Id,
                           Name = customer.Name,
                           Transaction = transaction
                       })
                    .AsNoTracking()
                    .ToList();

                var result = customers
                    .GroupBy(x => x.Id)
                    .Select(x => {
                        var pointForLastThreeMonth = x
                            .Where(t => t.Transaction != null && DateTime.Compare(t.Transaction.Date, DateTime.Today.AddMonths(-3)) >= 0)
                            .Select(t => t.Transaction);

                        return new CustomerView()
                        {
                            Id = x.First().Id,
                            Name = x.First().Name,
                            PointsAmountPerMonth = pointForLastThreeMonth
                                .OrderByDescending(p => p.Date)
                                .GroupBy(p => new { p.Date.Year, p.Date.Month })
                                .Select(p =>
                                    new TransactionPerMonthView()
                                    {
                                        Year = p.Key.Year,
                                        Month = p.Key.Month,
                                        Points = p.Sum(s => s.Points)
                                    }
                                ),
                            PointsAmount = pointForLastThreeMonth.Sum(x => x.Points),
                            Transactions = pointForLastThreeMonth
                                .Select(t => _mapper.Map<TransactionView>(t))
                        };
                    });

                _logger.LogInformation($"All customers returned successfully. Number of customers = {result.Count()}.");

                return result;
            }
            catch (Exception ex)
            {
                var errorMessage = "An error occurred while returning all customers.";
                _logger.LogError(ex, errorMessage);
                throw new DomainException(errorMessage);
            }
            
        }
    }
}
