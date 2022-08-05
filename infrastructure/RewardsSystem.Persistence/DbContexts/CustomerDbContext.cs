using Microsoft.EntityFrameworkCore;
using RewardsSystem.Domain.Entities;
using RewardsSystem.Shared.Interfaces;

namespace RewardsSystem.Persistence.DbContexts;

public class CustomerDbContext : DbContext, ICustomerDbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}
