using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RewardsSystem.Common.Mapping;
using RewardsSystem.Persistence.DbContexts;
using RewardsSystem.Tests.Mock;

namespace RewardsSystem.Tests.Services;

public class TestScopeBase<TRequest, TResponse, TRequestHandler>
    where TRequest : IRequest<TResponse>
    where TRequestHandler : IRequestHandler<TRequest, TResponse>
{
    public static CancellationToken AnyCancellationToken => It.IsAny<CancellationToken>();

    public TRequest Request { get; set; }
    public TResponse ExpectedResult { get; set; }
    public TRequestHandler Instance { get; set; }

    public CustomerDbContext DbContext { get; set; }

    public MapperConfiguration MockMapper { get; set; }
    public IMapper Mapper { get; set; }

    public ILogger<TRequestHandler> Logger { get; set; }

    public TestScopeBase()
    {
        var _options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        DbContext = new CustomerDbContext(_options);
        DbContext.Database.EnsureCreated();
        DbContext.Customers.AddRange(CustomersMock.Get());
        DbContext.Transactions.AddRange(TransactionsMock.Get());
        DbContext.SaveChanges();

        MockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile());
        });
        Mapper = MockMapper.CreateMapper();
        Logger = new NullLogger<TRequestHandler>();
    }
}
